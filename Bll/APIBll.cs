
using Entity;
using IBll;
using Microsoft.AspNetCore.Mvc.Formatters;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using WebCarProject.Models;
using Common;
using FFmpeg.AutoGen;
using System.Runtime.InteropServices;

namespace Bll
{
    
    public class APIBll:IAPIBll
    {

        public ISqlSugarClient db;

        #region 推流
        int mDstVideoWidth = 1280;//转码后默认分辨率宽
        int mDstVideoHeight = 720;//转码后默认分辨率高
        int mDstVideoIndex = -1;
        int mDstVideoFps = 25;//转码后默认fps
        int mDstVideoChannel = 1;
        #endregion
        public APIBll(ISqlSugarClient datadb)
        {
            this.db = datadb;
        }

        public void LJFUQ(string outputRtspUrl, byte[] yuvData)
        {
            unsafe
            {
                // 初始化libavformat和注册所有的复用器和解复用器和协议。
                ffmpeg.av_register_all();
                // 全局地初始化网络组件，需要用到网络功能的时候需要调用。
                ffmpeg.avformat_network_init();
                AVFormatContext* mDstFmtCtx = null;
                AVCodecContext* mDstVideoCodecCtx = null;
                AVStream* mDstVideoStream = null;
                //int mDstVideoIndex = -1;
                //int mDstVideoFps = 25;//转码后默认fps
                //int mDstVideoWidth = 1280;//转码后默认分辨率宽
                //int mDstVideoHeight = 720;//转码后默认分辨率高
                //int mDstVideoChannel = 1;

                if (ffmpeg.avformat_alloc_output_context2(&mDstFmtCtx, null, "rtsp", outputRtspUrl) < 0)
                {
                    Debug.WriteLine("avformat_alloc_output_context2 error: dstUrl=%s", outputRtspUrl);
                    return;
                }

                // init video start
                AVCodec* videoCodec = ffmpeg.avcodec_find_encoder(AVCodecID.AV_CODEC_ID_H264);
                if (videoCodec == null)
                {
                    Debug.WriteLine("avcodec_find_encoder error: dstUrl=%s", outputRtspUrl);
                    return;
                }
                mDstVideoCodecCtx = ffmpeg.avcodec_alloc_context3(videoCodec);
                if (mDstVideoCodecCtx == null)
                {
                    Debug.WriteLine("avcodec_alloc_context3 error: dstUrl=%s", outputRtspUrl);
                    return;
                }
                //int bit_rate = 300 * 1024 * 8;  //压缩后每秒视频的bit位大小 300kB
                int bit_rate = 4096000;
                // CBR：Constant BitRate - 固定比特率
                mDstVideoCodecCtx->flags |= ffmpeg.AV_CODEC_FLAG_QSCALE;
                mDstVideoCodecCtx->bit_rate = bit_rate;
                mDstVideoCodecCtx->rc_min_rate = bit_rate;
                mDstVideoCodecCtx->rc_max_rate = bit_rate;
                mDstVideoCodecCtx->bit_rate_tolerance = bit_rate;

                mDstVideoCodecCtx->codec_id = videoCodec->id;
                mDstVideoCodecCtx->pix_fmt = AVPixelFormat.AV_PIX_FMT_YUVJ420P;// 不支持AV_PIX_FMT_BGR24直接进行编码
                mDstVideoCodecCtx->codec_type = AVMediaType.AVMEDIA_TYPE_VIDEO;
                mDstVideoCodecCtx->width = mDstVideoWidth;
                mDstVideoCodecCtx->height = mDstVideoHeight;
                mDstVideoCodecCtx->time_base = new AVRational { num = 1, den = mDstVideoFps };
                //        mDstVideoCodecCtx->framerate = { mDstVideoFps, 1 };
                mDstVideoCodecCtx->gop_size = 5;
                mDstVideoCodecCtx->max_b_frames = 0;
                mDstVideoCodecCtx->thread_count = 1;
                mDstVideoCodecCtx->flags |= ffmpeg.AV_CODEC_FLAG_GLOBAL_HEADER;   //添加PPS、SPS
                AVDictionary* video_codec_options = null;

                //H.264
                if (mDstVideoCodecCtx->codec_id == AVCodecID.AV_CODEC_ID_H264)
                {
                    //            av_dict_set(&video_codec_options, "profile", "main", 0);
                    ffmpeg.av_dict_set(&video_codec_options, "preset", "superfast", 0);
                    ffmpeg.av_dict_set(&video_codec_options, "tune", "zerolatency", 0);
                }
                if (ffmpeg.avcodec_open2(mDstVideoCodecCtx, videoCodec, &video_codec_options) < 0)
                {
                    Debug.WriteLine("avcodec_open2 error: dstUrl=%s", outputRtspUrl);
                    return;
                }
                mDstVideoStream = ffmpeg.avformat_new_stream(mDstFmtCtx, videoCodec);
                if (mDstVideoStream == null)
                {
                    Debug.WriteLine("avformat_new_stream error: dstUrl=%s", outputRtspUrl);
                    return;
                }
                mDstVideoStream->id = (int)mDstFmtCtx->nb_streams - 1;
                // stream的time_base参数非常重要，它表示将现实中的一秒钟分为多少个时间基, 在下面调用avformat_write_header时自动完成
                ffmpeg.avcodec_parameters_from_context(mDstVideoStream->codecpar, mDstVideoCodecCtx);
                mDstVideoIndex = mDstVideoStream->id;
                // init video end

                ffmpeg.av_dump_format(mDstFmtCtx, 0, outputRtspUrl, 1);

                // open output url
                if ((mDstFmtCtx->oformat->flags & ffmpeg.AVFMT_NOFILE) == 0)
                {
                    if (ffmpeg.avio_open(&mDstFmtCtx->pb, outputRtspUrl, ffmpeg.AVIO_FLAG_WRITE) < 0)
                    {
                        Debug.WriteLine("avio_open error: dstUrl=%s", outputRtspUrl);
                        return;
                    }
                }
                AVDictionary* fmt_options = null;
                //av_dict_set(&fmt_options, "bufsize", "1024", 0);
                ffmpeg.av_dict_set(&fmt_options, "rw_timeout", "30000000", 0); //设置rtmp/http-flv连接超时（单位 us）
                ffmpeg.av_dict_set(&fmt_options, "stimeout", "30000000", 0);   //设置rtsp连接超时（单位 us）
                ffmpeg.av_dict_set(&fmt_options, "rtsp_transport", "tcp", 0);
                //        av_dict_set(&fmt_options, "fflags", "discardcorrupt", 0);

                //av_dict_set(&fmt_options, "muxdelay", "0.1", 0);
                //av_dict_set(&fmt_options, "tune", "zerolatency", 0);

                mDstFmtCtx->video_codec_id = mDstFmtCtx->oformat->video_codec;

                if (ffmpeg.avformat_write_header(mDstFmtCtx, &fmt_options) < 0)
                { // 调用该函数会将所有stream的time_base，自动设置一个值，通常是1/90000或1/1000，这表示一秒钟表示的时间基长度
                    Debug.WriteLine("avformat_write_header error: dstUrl=%s", outputRtspUrl);
                    return;
                }
                SPTL(yuvData, mDstVideoCodecCtx, mDstFmtCtx);
            }
                 
        }
        public unsafe void SPTL(byte[] yuvData, AVCodecContext* mDstVideoCodecCtx, AVFormatContext* mDstFmtCtx)
        {
            unsafe
            {
                // 推流部分                    
                int frameSize = yuvData.Length;
                AVFrame* frame = ffmpeg.av_frame_alloc();
                if (frame == null)
                {
                    Debug.WriteLine("Unable to allocate AVFrame");
                }
                //ame->format = (int)mDstVideoCodecCtx->pix_fmt;
                frame->width = mDstVideoWidth;
                frame->height = mDstVideoHeight;
                int ret = ffmpeg.av_frame_get_buffer(frame, 32);
                if (ret < 0)
                {
                    Debug.WriteLine("av_frame_get_buffer error");
                    return;
                }
                for (int i = 0; i < frameSize; i++)
                {
                    frame->data[0][i] = yuvData[i]; // Y plane
                }
                AVPacket packet = new AVPacket();
                ffmpeg.av_init_packet(&packet);
                packet.data = null;
                packet.size = 0;

                ret = ffmpeg.avcodec_send_frame(mDstVideoCodecCtx, frame);
                if (ret < 0)
                {
                    Debug.WriteLine("avcodec_send_frame error");
                    return;
                }

                ret = ffmpeg.avcodec_receive_packet(mDstVideoCodecCtx, &packet);
                if (ret < 0)
                {
                    Debug.WriteLine("avcodec_receive_packet error");
                    return;
                }

                ret = ffmpeg.av_interleaved_write_frame(mDstFmtCtx, &packet);
                if (ret < 0)
                {
                    Debug.WriteLine("av_interleaved_write_frame error");
                    return;
                }

                // 清理资源
                ffmpeg.av_packet_unref(&packet);
                ffmpeg.av_frame_free(&frame);
                ffmpeg.avcodec_close(mDstVideoCodecCtx);
                ffmpeg.avformat_free_context(mDstFmtCtx);
            }
        }
        /// <summary>
        /// 服务器视频
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

       public void  FWQSP()
        { 
              InitCamera(out string msg); 
        }
        HZCameraControl mCameraControl = new HZCameraControl();
        /// <summary>
        //初始化SDK
        /// 
        /// </summary>
        public void InitCamera(out string msg)
        {
            //监控初始化
            if (!mCameraControl.Init())
            {
                //  MessageBox.Show("监控初始化失败");
                msg = "监控初始化失败";
                return ;
            }
            //用户登录  网络相机IP地址 端口号(默认9000) 登录名  密码
            if (!mCameraControl.Login("192.168.2.19", 9000, "admin", "lwny1234"))
            {
                //  MessageBox.Show("用户登录失败");
                msg = "用户登录失败";
                return ;
            }

            //设置解码回调
            mCameraControl.mDecCBFunc = DecodeCallBack;

            //获取监控数据
            if (!mCameraControl.OpenPlay())
            {
                msg = "获取监控数据失败";
                return ;
            }
            msg = "";
            return ;
        }
       public void DecodeCallBack(int nPort, IntPtr pBuf, int nSize, HZ_PLAY.FRAME_INFO pFrameInfo, IntPtr pUserData, int nReserved2)
        {
            //限制画面刷新
            mCameraControl.isUpdate = false;
            if (pFrameInfo.nType == 3)
            {
                //监控视频YUV流转RGB图片
                int width = pFrameInfo.nWidth;
                int height = pFrameInfo.nHeight;
                if (width <= 0 || height <= 0)
                    return;
                byte[] yuvbytes = new byte[nSize];
                Marshal.Copy(pBuf, yuvbytes, 0, yuvbytes.Length);
                string dst = "rtsp://8.137.119.17:554/live/test1";
                LJFUQ(dst, yuvbytes);
            }
        }
    }
}
