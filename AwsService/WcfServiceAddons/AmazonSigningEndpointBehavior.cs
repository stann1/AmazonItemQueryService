using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace AwsService.WcfServiceAddons
{
    class AmazonSigningEndpointBehavior : IEndpointBehavior
    {
        private readonly string _accessKeyId = "";
        private readonly string _secretKey = "";

        public AmazonSigningEndpointBehavior(string accessKeyId, string secretKey)
        {
            this._accessKeyId = accessKeyId;
            this._secretKey = secretKey;
        }

        public void ApplyClientBehavior(ServiceEndpoint serviceEndpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new AmazonSigningMessageInspector(_accessKeyId, _secretKey));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint serviceEndpoint, EndpointDispatcher endpointDispatcher) { return; }
        public void Validate(ServiceEndpoint serviceEndpoint) { return; }
        public void AddBindingParameters(ServiceEndpoint serviceEndpoint, BindingParameterCollection bindingParameters) { return; }
    }
}
