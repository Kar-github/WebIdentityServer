using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiDemo.Services.Infrastructure.Requests
{
    public abstract class BaseRequest<TResponse> : IRequest<TResponse>
    {
        private readonly Dictionary<string, string> _additionalData;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool UsesIdempotent { get; private set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool UsesConnection { get; private set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool UsesTransaction { get; private set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool UsesEventManager { get; private set; }
        internal virtual void SetIdempotent(bool isIdempotent) => UsesIdempotent = isIdempotent;
        internal virtual void SetUsesConnection(bool usesConnection) => UsesConnection = usesConnection;
        internal virtual void SetUsesTransaction(bool usesTransaction) => UsesTransaction = usesTransaction;
        internal virtual void SetUsesEventManager(bool usesEventManager) => UsesEventManager = usesEventManager;
        public BaseRequest()
        {
            _additionalData = new Dictionary<string, string>();
            UsesIdempotent = true;
            UsesConnection = true;
            UsesTransaction = true;
            UsesEventManager = true;
        }
        public BaseRequest(bool usesIdempotent, bool usesTransaction, bool usesConnection) : this()
        {
            UsesIdempotent = usesIdempotent;
            UsesTransaction = usesTransaction;
            UsesConnection = usesConnection;
        }
        public void AddAdditionalData<T>(string key, T value)
        {
            var valueJson = JsonConvert.SerializeObject(value);
            _additionalData.Add(key, valueJson);
        }
        public void AddAdditionalData(string key, string value)
        {
            _additionalData.Add(key, value);
        }
        public Dictionary<string, string> GetAdditionalData() => _additionalData;
        public string GetAdditionalData(string key) => _additionalData.ContainsKey(key) ? _additionalData[key] : "";
        public T GetAdditionalData<T>(string key) => _additionalData.ContainsKey(key) ? JsonConvert.DeserializeObject<T>(_additionalData[key]) : default;
    }
}
