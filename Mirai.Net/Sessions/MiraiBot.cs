﻿using System;
using System.Threading.Tasks;
using AHpx.Extensions.JsonExtensions;
using AHpx.Extensions.StringExtensions;
using AHpx.Extensions.Utils;
using Mirai.Net.Utils.Extensions;

namespace Mirai.Net.Sessions
{
    /// <summary>
    /// Mirai机器人
    /// </summary>
    public class MiraiBot : IDisposable
    {
        public MiraiBot(string address = null, string verifyKey = null, long qq = default, string sessionKey = null)
        {
            _address = address;
            VerifyKey = verifyKey;
            SessionKey = sessionKey;
            QQ = qq;
        }

        /// <summary>
        /// 启动bot对象
        /// </summary>
        public async Task Launch()
        {
            await LaunchHttpAdapter();
        }

        #region Adapter launcher

        private async Task LaunchHttpAdapter()
        {
            SessionKey = await GetSessionKey();
            await BindQqToSession();
        }

        #endregion

        #region Property definitions

        /// <summary>
        /// Mirai.Net总是需要一个VerifyKey
        /// </summary>
        public string VerifyKey { get; set; }

        /// <summary>
        /// 新建连接 或 singleMode 模式下为空, 通过已有 sessionKey 连接时不可为空
        /// </summary>
        public string SessionKey { get; set; }

        private string _address;
        /// <summary>
        /// 比如：localhost:114514
        /// </summary>
        public string Address
        {
            get => _address.TrimEnd('/').Empty("http://").Empty("https://");
            set => _address = value;
        }

        /// <summary>
        /// 绑定的账号, singleMode 模式下为空, 非 singleMode 下新建连接不可为空
        /// </summary>
        public long QQ { get; set; }

        #endregion

        #region Http adapter

        /// <summary>
        /// 调用端点: /verify，返回session key
        /// </summary>
        /// <returns>返回sessionKey</returns>
        private async Task<string> GetSessionKey()
        {
            var url = this.GetUrl("verify");
            var response = await HttpUtilities.PostJsonAsync(url, new
            {
                verifyKey = VerifyKey
            }.ToJsonString());

            await this.EnsureSuccess(response);

            var content = await response.FetchContent();

            return content.Fetch("session");
        }

        /// <summary>
        /// 调用端点：/bind，将当前对象的qq好绑定的指定的sessionKey
        /// </summary>
        private async Task BindQqToSession()
        {
            var url = this.GetUrl("bind");
            var response = await HttpUtilities.PostJsonAsync(url, new
            {
                sessionKey = SessionKey,
                qq = QQ
            }.ToJsonString());

            await this.EnsureSuccess(response);
        }

        /// <summary>
        /// 调用端口：/release，释放bot的资源占用
        /// </summary>
        private async Task ReleaseOccupy()
        {
            var url = this.GetUrl("release");
            var response = await HttpUtilities.PostJsonAsync(url, new
            {
                sessionKey = SessionKey,
                qq = QQ
            }.ToJsonString());

            await this.EnsureSuccess(response);
        }

        #endregion

        #region Diagnose stuff

        /// <summary>
        /// 重写了默认的ToString方法，本质上是替代为ToJsonString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToJsonString();
        }

        /// <summary>
        /// 释放MiraiBot对象占用的资源
        /// </summary>
        public async void Dispose()
        {
            await ReleaseOccupy();
        }

        #endregion
    }
}