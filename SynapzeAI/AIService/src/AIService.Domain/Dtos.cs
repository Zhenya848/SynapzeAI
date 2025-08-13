/* Options:
Date: 2025-08-03 18:23:19
Version: 8.73
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: https://openai.servicestack.net

//GlobalNamespace: 
//MakePartial: True
//MakeVirtual: True
//MakeInternal: False
//MakeDataContractsExtensible: False
//AddNullableAnnotations: False
//AddReturnMarker: True
//AddDescriptionAsComments: True
//AddDataContractAttributes: False
//AddIndexesToDataMembers: False
//AddGeneratedCodeAttributes: False
//AddResponseStatus: False
//AddImplicitVersion: 
//InitializeCollections: False
//ExportValueTypes: False
//IncludeTypes: 
//ExcludeTypes: 
//AddNamespaces: 
//AddDefaultXmlNamespace: http://schemas.servicestack.net/types
*/

using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Jobs;

namespace AIService.Domain;

///<summary>
///Active AI Worker Models available in AI Server
///</summary>
[Api(Description="Active AI Worker Models available in AI Server")]
public partial class ActiveAiModels
    : IReturn<StringsResponse>, IGet
{
    public virtual AiProviderType? Provider { get; set; }
    public virtual bool? Vision { get; set; }
}

///<summary>
///Active Custom AI Worker Models available in AI Server
///</summary>
[Api(Description="Active Custom AI Worker Models available in AI Server")]
public partial class ActiveCustomAiModels
    : IReturn<StringsResponse>, IGet
{
}

///<summary>
///Active Media Worker Models available in AI Server
///</summary>
[Api(Description="Active Media Worker Models available in AI Server")]
public partial class ActiveMediaModels
    : IReturn<ActiveMediaModelsResponse>, IGet
{
}

public partial class ActiveMediaModelsResponse
{
    public virtual List<Entry> Results { get; set; } = [];
    public virtual ResponseStatus ResponseStatus { get; set; }
}

public partial class AdminData
    : IReturn<AdminDataResponse>, IGet
{
}

public partial class AdminDataResponse
{
    public virtual List<PageStats> PageStats { get; set; } = [];
}

public partial class AiModel
{
    public virtual string Id { get; set; }
    public virtual List<string> Tags { get; set; } = [];
    public virtual string Latest { get; set; }
    public virtual string Website { get; set; }
    public virtual string Description { get; set; }
    public virtual string Icon { get; set; }
    public virtual bool? Vision { get; set; }
}

public partial class AiProvider
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string ApiBaseUrl { get; set; }
    public virtual string ApiKeyVar { get; set; }
    public virtual string ApiKey { get; set; }
    public virtual string ApiKeyHeader { get; set; }
    public virtual string HeartbeatUrl { get; set; }
    public virtual int Concurrency { get; set; }
    public virtual int Priority { get; set; }
    public virtual bool Enabled { get; set; }
    public virtual DateTime? OfflineDate { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual List<AiProviderModel> Models { get; set; } = [];
    public virtual string AiTypeId { get; set; }
    [Ignore]
    public virtual AiType AiType { get; set; }

    [Ignore]
    public virtual List<string> SelectedModels { get; set; } = [];
}

public partial class AiProviderFileOutput
{
    public virtual string FileName { get; set; }
    public virtual string Url { get; set; }
}

public partial class AiProviderModel
{
    public virtual string Model { get; set; }
    public virtual string ApiModel { get; set; }
}

public partial class AiProviderTextOutput
{
    public virtual string Text { get; set; }
}

public enum AiProviderType
{
    OllamaAiProvider,
    OpenAiProvider,
    GoogleAiProvider,
    AnthropicAiProvider,
    CustomOpenAiProvider,
}

public enum AiTaskType
{
    TextToImage = 1,
    ImageToImage = 2,
    ImageUpscale = 3,
    ImageWithMask = 4,
    ImageToText = 5,
    TextToAudio = 6,
    TextToSpeech = 7,
    SpeechToText = 8,
}

public partial class AiType
{
    public virtual string Id { get; set; }
    public virtual AiProviderType Provider { get; set; }
    public virtual string Website { get; set; }
    public virtual string ApiBaseUrl { get; set; }
    public virtual string HeartbeatUrl { get; set; }
    public virtual string Icon { get; set; }
    public virtual Dictionary<string, string> ApiModels { get; set; } = new();
}

///<summary>
///Response object for artifact generation requests
///</summary>
[Api(Description="Response object for artifact generation requests")]
public partial class ArtifactGenerationResponse
{
    ///<summary>
    ///List of generated outputs
    ///</summary>
    [ApiMember(Description="List of generated outputs")]
    public virtual List<ArtifactOutput> Results { get; set; }

    ///<summary>
    ///Detailed response status information
    ///</summary>
    [ApiMember(Description="Detailed response status information")]
    public virtual ResponseStatus ResponseStatus { get; set; }
}

public partial class ArtifactOutput
{
    ///<summary>
    ///URL to access the generated image
    ///</summary>
    [ApiMember(Description="URL to access the generated image")]
    public virtual string Url { get; set; }

    ///<summary>
    ///Filename of the generated image
    ///</summary>
    [ApiMember(Description="Filename of the generated image")]
    public virtual string FileName { get; set; }

    ///<summary>
    ///Provider used for image generation
    ///</summary>
    [ApiMember(Description="Provider used for image generation")]
    public virtual string Provider { get; set; }
}

[DataContract]
public enum AudioFormat
{
    [EnumMember(Value="mp3")]
    MP3,
    [EnumMember(Value="wav")]
    WAV,
    [EnumMember(Value="aac")]
    AAC,
    [EnumMember(Value="flac")]
    FLAC,
    [EnumMember(Value="ogg")]
    OGG,
    [EnumMember(Value="m4a")]
    M4A,
    [EnumMember(Value="wma")]
    WMA,
}

public partial class CancelWorker
    : IReturn<EmptyResponse>
{
    [Validate("NotEmpty")]
    public virtual string Worker { get; set; }
}

public partial class ChangeAiProviderStatus
    : IReturn<StringResponse>, IPost
{
    public virtual string Provider { get; set; }
    public virtual bool Online { get; set; }
}

public partial class ChatAiProvider
    : IReturn<OpenAiChatResponse>, IPost
{
    public virtual string Provider { get; set; }
    public virtual string Model { get; set; }
    public virtual OpenAiChat Request { get; set; }
    public virtual string Prompt { get; set; }
}

public partial class CheckAiProviderStatus
    : IReturn<BoolResponse>, IPost
{
    public virtual string Provider { get; set; }
}

public partial class CheckMediaProviderStatus
    : IReturn<BoolResponse>, IPost
{
    public virtual string Provider { get; set; }
}

public partial class Choice
{
    ///<summary>
    ///The reason the model stopped generating tokens. This will be stop if the model hit a natural stop point or a provided stop sequence, length if the maximum number of tokens specified in the request was reached, content_filter if content was omitted due to a flag from our content filters, tool_calls if the model called a tool
    ///</summary>
    [DataMember(Name="finish_reason")]
    [ApiMember(Description="The reason the model stopped generating tokens. This will be stop if the model hit a natural stop point or a provided stop sequence, length if the maximum number of tokens specified in the request was reached, content_filter if content was omitted due to a flag from our content filters, tool_calls if the model called a tool")]
    public virtual string FinishReason { get; set; }

    ///<summary>
    ///The index of the choice in the list of choices.
    ///</summary>
    [DataMember(Name="index")]
    [ApiMember(Description="The index of the choice in the list of choices.")]
    public virtual int Index { get; set; }

    ///<summary>
    ///A chat completion message generated by the model.
    ///</summary>
    [DataMember(Name="message")]
    [ApiMember(Description="A chat completion message generated by the model.")]
    public virtual ChoiceMessage Message { get; set; }
}

[DataContract]
public partial class ChoiceMessage
{
    ///<summary>
    ///The contents of the message.
    ///</summary>
    [DataMember(Name="content")]
    [ApiMember(Description="The contents of the message.")]
    public virtual string Content { get; set; }

    ///<summary>
    ///The tool calls generated by the model, such as function calls.
    ///</summary>
    [DataMember(Name="tool_calls")]
    [ApiMember(Description="The tool calls generated by the model, such as function calls.")]
    public virtual List<ToolCall> ToolCalls { get; set; }

    ///<summary>
    ///The role of the author of this message.
    ///</summary>
    [DataMember(Name="role")]
    [ApiMember(Description="The role of the author of this message.")]
    public virtual string Role { get; set; }
}

///<summary>
///Convert an audio file to a different format
///</summary>
[Api(Description="Convert an audio file to a different format")]
public partial class ConvertAudio
    : IReturn<ArtifactGenerationResponse>, IMediaTransform
{
    ///<summary>
    ///The desired output format for the converted audio
    ///</summary>
    [ApiMember(Description="The desired output format for the converted audio")]
    [Required]
    public virtual AudioFormat OutputFormat { get; set; }

    [Required]
    public virtual string Audio { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Convert an image to a different format
///</summary>
[Api(Description="Convert an image to a different format")]
public partial class ConvertImage
    : IReturn<ArtifactGenerationResponse>, IMediaTransform, IPost
{
    ///<summary>
    ///The image file to be converted
    ///</summary>
    [ApiMember(Description="The image file to be converted")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///The desired output format for the converted image
    ///</summary>
    [ApiMember(Description="The desired output format for the converted image")]
    [Required]
    public virtual ImageOutputFormat? OutputFormat { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Convert a video to a different format
///</summary>
[Api(Description="Convert a video to a different format")]
public partial class ConvertVideo
    : IReturn<ArtifactGenerationResponse>, IMediaTransform
{
    ///<summary>
    ///The desired output format for the converted video
    ///</summary>
    [ApiMember(Description="The desired output format for the converted video")]
    [Required]
    public virtual ConvertVideoOutputFormat OutputFormat { get; set; }

    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

[DataContract]
public enum ConvertVideoOutputFormat
{
    [EnumMember(Value="mp4")]
    MP4,
    [EnumMember(Value="mov")]
    MOV,
    [EnumMember(Value="webm")]
    WebM,
    [EnumMember(Value="mkv")]
    MVK,
    [EnumMember(Value="avi")]
    AVI,
    [EnumMember(Value="wmv")]
    WMV,
    [EnumMember(Value="mpeg")]
    MPEG,
    [EnumMember(Value="ogg")]
    Ogg,
}

///<summary>
///Add an AI Provider to process AI Requests
///</summary>
[Api(Description="Add an AI Provider to process AI Requests")]
public partial class CreateAiProvider
    : IReturn<IdResponse>, ICreateDb<AiProvider>
{
    ///<summary>
    ///The Type of this API Provider
    ///</summary>
    [Validate("GreaterThan(0)")]
    [ApiMember(Description="The Type of this API Provider")]
    public virtual string AiTypeId { get; set; }

    ///<summary>
    ///The Base URL for the API Provider
    ///</summary>
    [ApiMember(Description="The Base URL for the API Provider")]
    public virtual string ApiBaseUrl { get; set; }

    ///<summary>
    ///The unique name for this API Provider
    ///</summary>
    [Validate("NotEmpty")]
    [ApiMember(Description="The unique name for this API Provider")]
    public virtual string Name { get; set; }

    ///<summary>
    ///The API Key to use for this Provider
    ///</summary>
    [ApiMember(Description="The API Key to use for this Provider")]
    public virtual string ApiKeyVar { get; set; }

    ///<summary>
    ///The API Key to use for this Provider
    ///</summary>
    [ApiMember(Description="The API Key to use for this Provider")]
    public virtual string ApiKey { get; set; }

    ///<summary>
    ///Send the API Key in the Header instead of Authorization Bearer
    ///</summary>
    [ApiMember(Description="Send the API Key in the Header instead of Authorization Bearer")]
    public virtual string ApiKeyHeader { get; set; }

    ///<summary>
    ///The URL to check if the API Provider is still online
    ///</summary>
    [ApiMember(Description="The URL to check if the API Provider is still online")]
    public virtual string HeartbeatUrl { get; set; }

    ///<summary>
    ///Override API Paths for different AI Requests
    ///</summary>
    [ApiMember(Description="Override API Paths for different AI Requests")]
    public virtual Dictionary<TaskType, string> TaskPaths { get; set; }

    ///<summary>
    ///How many requests should be made concurrently
    ///</summary>
    [ApiMember(Description="How many requests should be made concurrently")]
    public virtual int Concurrency { get; set; }

    ///<summary>
    ///What priority to give this Provider to use for processing models
    ///</summary>
    [ApiMember(Description="What priority to give this Provider to use for processing models")]
    public virtual int Priority { get; set; }

    ///<summary>
    ///Whether the Provider is enabled
    ///</summary>
    [ApiMember(Description="Whether the Provider is enabled")]
    public virtual bool Enabled { get; set; }

    ///<summary>
    ///The models this API Provider should process
    ///</summary>
    [ApiMember(Description="The models this API Provider should process")]
    public virtual List<AiProviderModel> Models { get; set; }

    ///<summary>
    ///The selected models this API Provider should process
    ///</summary>
    [ApiMember(Description="The selected models this API Provider should process")]
    public virtual List<string> SelectedModels { get; set; }
}

public partial class CreateApiKey
    : IReturn<CreateApiKeyResponse>, IPost
{
    public virtual string Key { get; set; }
    public virtual string Name { get; set; }
    public virtual string UserId { get; set; }
    public virtual string UserName { get; set; }
    public virtual List<string> Scopes { get; set; } = [];
    public virtual string Notes { get; set; }
    public virtual int? RefId { get; set; }
    public virtual string RefIdStr { get; set; }
    public virtual Dictionary<string, string> Meta { get; set; }
}

public partial class CreateApiKeyResponse
{
    public virtual int Id { get; set; }
    public virtual string Key { get; set; }
    public virtual string Name { get; set; }
    public virtual string UserId { get; set; }
    public virtual string UserName { get; set; }
    public virtual string VisibleKey { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual DateTime? ExpiryDate { get; set; }
    public virtual DateTime? CancelledDate { get; set; }
    public virtual string Notes { get; set; }
}

[Route("/generate", "POST")]
public partial class CreateGeneration
    : IReturn<CreateGenerationResponse>
{
    [Validate("NotNull")]
    public virtual GenerationArgs Request { get; set; }

    public virtual string Provider { get; set; }
    public virtual string State { get; set; }
    public virtual string ReplyTo { get; set; }
    public virtual string RefId { get; set; }
}

public partial class CreateGenerationResponse
{
    public virtual long Id { get; set; }
    public virtual string RefId { get; set; }
}

public partial class CreateMediaProvider
    : IReturn<IdResponse>, ICreateDb<MediaProvider>
{
    public virtual string Name { get; set; }
    public virtual string ApiKey { get; set; }
    public virtual string ApiKeyHeader { get; set; }
    public virtual string ApiBaseUrl { get; set; }
    public virtual string HeartbeatUrl { get; set; }
    ///<summary>
    ///How many requests should be made concurrently
    ///</summary>
    [ApiMember(Description="How many requests should be made concurrently")]
    public virtual int Concurrency { get; set; }

    ///<summary>
    ///What priority to give this Provider to use for processing models
    ///</summary>
    [ApiMember(Description="What priority to give this Provider to use for processing models")]
    public virtual int Priority { get; set; }

    ///<summary>
    ///Whether the Provider is enabled
    ///</summary>
    [ApiMember(Description="Whether the Provider is enabled")]
    public virtual bool Enabled { get; set; }

    ///<summary>
    ///The date the Provider was last online
    ///</summary>
    [ApiMember(Description="The date the Provider was last online")]
    public virtual DateTime? OfflineDate { get; set; }

    ///<summary>
    ///Models this API Provider should process
    ///</summary>
    [ApiMember(Description="Models this API Provider should process")]
    public virtual List<string> Models { get; set; }

    public virtual string MediaTypeId { get; set; }
}

public partial class CreateMediaTransform
    : IReturn<CreateTransformResponse>
{
    [Validate("NotNull")]
    public virtual MediaTransformArgs Request { get; set; }

    public virtual string Provider { get; set; }
    public virtual string State { get; set; }
    public virtual string ReplyTo { get; set; }
    public virtual string RefId { get; set; }
}

public partial class CreateTransformResponse
{
    public virtual long Id { get; set; }
    public virtual string RefId { get; set; }
}

///<summary>
///Crop an image to a specified area
///</summary>
[Api(Description="Crop an image to a specified area")]
public partial class CropImage
    : IReturn<ArtifactGenerationResponse>, IMediaTransform, IPost
{
    ///<summary>
    ///The X-coordinate of the top-left corner of the crop area
    ///</summary>
    [ApiMember(Description="The X-coordinate of the top-left corner of the crop area")]
    public virtual int X { get; set; }

    ///<summary>
    ///The Y-coordinate of the top-left corner of the crop area
    ///</summary>
    [ApiMember(Description="The Y-coordinate of the top-left corner of the crop area")]
    public virtual int Y { get; set; }

    ///<summary>
    ///The width of the crop area
    ///</summary>
    [ApiMember(Description="The width of the crop area")]
    public virtual int Width { get; set; }

    ///<summary>
    ///The height of the crop area
    ///</summary>
    [ApiMember(Description="The height of the crop area")]
    public virtual int Height { get; set; }

    ///<summary>
    ///The image file to be cropped
    ///</summary>
    [ApiMember(Description="The image file to be cropped")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Crop a video to a specified area
///</summary>
[Api(Description="Crop a video to a specified area")]
public partial class CropVideo
    : IReturn<ArtifactGenerationResponse>, IMediaTransform
{
    ///<summary>
    ///The X-coordinate of the top-left corner of the crop area
    ///</summary>
    [ApiMember(Description="The X-coordinate of the top-left corner of the crop area")]
    [Validate("GreaterThan(0)")]
    [Required]
    public virtual int X { get; set; }

    ///<summary>
    ///The Y-coordinate of the top-left corner of the crop area
    ///</summary>
    [ApiMember(Description="The Y-coordinate of the top-left corner of the crop area")]
    [Validate("GreaterThan(0)")]
    [Required]
    public virtual int Y { get; set; }

    ///<summary>
    ///The width of the crop area
    ///</summary>
    [ApiMember(Description="The width of the crop area")]
    [Validate("GreaterThan(0)")]
    [Required]
    public virtual int Width { get; set; }

    ///<summary>
    ///The height of the crop area
    ///</summary>
    [ApiMember(Description="The height of the crop area")]
    [Validate("GreaterThan(0)")]
    [Required]
    public virtual int Height { get; set; }

    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Delete API Provider
///</summary>
[Api(Description="Delete API Provider")]
public partial class DeleteAiProvider
    : IReturnVoid, IDeleteDb<AiProvider>
{
    public virtual int Id { get; set; }
}

[Route("/files/{**Path}")]
public partial class DeleteFile
    : IReturn<EmptyResponse>, IDelete
{
    [Validate("NotEmpty")]
    public virtual string Path { get; set; }
}

public partial class DeleteFiles
    : IReturn<DeleteFilesResponse>, IPost
{
    public virtual List<string> Paths { get; set; } = [];
}

public partial class DeleteFilesResponse
{
    public virtual List<string> Deleted { get; set; } = [];
    public virtual List<string> Missing { get; set; } = [];
    public virtual List<string> Failed { get; set; } = [];
    public virtual ResponseStatus ResponseStatus { get; set; }
}

///<summary>
///Delete a Generation API Provider
///</summary>
[Api(Description="Delete a Generation API Provider")]
public partial class DeleteMediaProvider
    : IReturn<IdResponse>, IDeleteDb<MediaProvider>
{
    public virtual int? Id { get; set; }
    public virtual string Name { get; set; }
}

public partial class Entry
{
    public virtual string Key { get; set; }
    public virtual string Value { get; set; }
}

public partial class GenerationArgs
{
    public virtual string Model { get; set; }
    public virtual int? Steps { get; set; }
    public virtual int? BatchSize { get; set; }
    public virtual int? Seed { get; set; }
    public virtual string PositivePrompt { get; set; }
    public virtual string NegativePrompt { get; set; }
    public virtual Stream ImageInput { get; set; }
    public virtual Stream MaskInput { get; set; }
    public virtual Stream AudioInput { get; set; }
    public virtual ComfySampler? Sampler { get; set; }
    public virtual string Scheduler { get; set; }
    public virtual double? CfgScale { get; set; }
    public virtual double? Denoise { get; set; }
    public virtual string UpscaleModel { get; set; }
    public virtual int? Width { get; set; }
    public virtual int? Height { get; set; }
    public virtual AiTaskType? TaskType { get; set; }
    public virtual string Clip { get; set; }
    public virtual double? SampleLength { get; set; }
    public virtual ComfyMaskSource MaskChannel { get; set; }
    public virtual string AspectRatio { get; set; }
    public virtual double? Quality { get; set; }
    public virtual string Voice { get; set; }
    public virtual string Language { get; set; }
}

public partial class GenerationResult
{
    public virtual List<AiProviderTextOutput> TextOutputs { get; set; }
    public virtual List<AiProviderFileOutput> Outputs { get; set; }
    public virtual string Error { get; set; }
}

public partial class GetActiveProviders
    : IReturn<GetActiveProvidersResponse>, IGet
{
}

public partial class GetActiveProvidersResponse
{
    public virtual List<AiProvider> Results { get; set; } = [];
    public virtual ResponseStatus ResponseStatus { get; set; }
}

[Route("/artifacts/{**Path}")]
public partial class GetArtifact
    : IReturn<byte[]>, IGet
{
    [Validate("NotEmpty")]
    public virtual string Path { get; set; }

    public virtual bool? Download { get; set; }
}

///<summary>
///Get job status
///</summary>
[Api(Description="Get job status")]
public partial class GetArtifactGenerationStatus
    : IReturn<GetArtifactGenerationStatusResponse>, IGet
{
    ///<summary>
    ///Unique identifier of the background job
    ///</summary>
    [ApiMember(Description="Unique identifier of the background job")]
    public virtual long? JobId { get; set; }

    ///<summary>
    ///Client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Client-provided identifier for the request")]
    public virtual string RefId { get; set; }
}

public partial class GetArtifactGenerationStatusResponse
{
    ///<summary>
    ///Unique identifier of the background job
    ///</summary>
    [ApiMember(Description="Unique identifier of the background job")]
    public virtual long JobId { get; set; }

    ///<summary>
    ///Client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Current state of the background job
    ///</summary>
    [ApiMember(Description="Current state of the background job")]
    public virtual BackgroundJobState JobState { get; set; }

    ///<summary>
    ///Current status of the generation request
    ///</summary>
    [ApiMember(Description="Current status of the generation request")]
    public virtual string Status { get; set; }

    ///<summary>
    ///List of generated outputs
    ///</summary>
    [ApiMember(Description="List of generated outputs")]
    public virtual List<ArtifactOutput> Results { get; set; }

    ///<summary>
    ///Detailed response status information
    ///</summary>
    [ApiMember(Description="Detailed response status information")]
    public virtual ResponseStatus ResponseStatus { get; set; }
}

public partial class GetComfyModelMappings
    : IReturn<GetComfyModelMappingsResponse>
{
}

public partial class GetComfyModelMappingsResponse
{
    public virtual Dictionary<string, string> Models { get; set; } = new();
}

public partial class GetComfyModels
    : IReturn<GetComfyModelsResponse>
{
    public virtual string ApiBaseUrl { get; set; }
    public virtual string ApiKey { get; set; }
}

public partial class GetComfyModelsResponse
{
    public virtual List<string> Results { get; set; } = [];
    public virtual ResponseStatus ResponseStatus { get; set; }
}

[Route("/generation/{Id}", "GET")]
[Route("/generation/ref/{RefId}", "GET")]
public partial class GetGeneration
    : IReturn<GetGenerationResponse>
{
    public virtual int? Id { get; set; }
    public virtual string RefId { get; set; }
}

public partial class GetGenerationResponse
{
    public virtual GenerationArgs Request { get; set; }
    public virtual GenerationResult Result { get; set; }
    public virtual List<AiProviderFileOutput> Outputs { get; set; }
    public virtual List<AiProviderTextOutput> TextOutputs { get; set; }
}

[Route("/icons/models/{Model}", "GET")]
public partial class GetModelImage
    : IReturn<byte[]>, IGet
{
    public virtual string Model { get; set; }
}

public partial class GetOllamaGenerationStatus
    : IReturn<GetOllamaGenerationStatusResponse>, IGet
{
    public virtual long JobId { get; set; }
    public virtual string RefId { get; set; }
}

public partial class GetOllamaGenerationStatusResponse
{
    ///<summary>
    ///Unique identifier of the background job
    ///</summary>
    [ApiMember(Description="Unique identifier of the background job")]
    public virtual long JobId { get; set; }

    ///<summary>
    ///Client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Current state of the background job
    ///</summary>
    [ApiMember(Description="Current state of the background job")]
    public virtual BackgroundJobState JobState { get; set; }

    ///<summary>
    ///Current status of the generation request
    ///</summary>
    [ApiMember(Description="Current status of the generation request")]
    public virtual string Status { get; set; }

    ///<summary>
    ///Detailed response status information
    ///</summary>
    [ApiMember(Description="Detailed response status information")]
    public virtual ResponseStatus ResponseStatus { get; set; }

    ///<summary>
    ///Generation result
    ///</summary>
    [ApiMember(Description="Generation result")]
    public virtual OllamaGenerateResponse Result { get; set; }
}

public partial class GetOllamaModels
    : IReturn<GetOllamaModelsResponse>, IGet
{
    [Validate("NotEmpty")]
    public virtual string ApiBaseUrl { get; set; }
}

public partial class GetOllamaModelsResponse
{
    public virtual List<OllamaModel> Results { get; set; } = [];
    public virtual ResponseStatus ResponseStatus { get; set; }
}

public partial class GetOpenAiChat
    : IReturn<GetOpenAiChatResponse>, IGet
{
    public virtual int? Id { get; set; }
    public virtual string RefId { get; set; }
}

public partial class GetOpenAiChatResponse
{
    public virtual BackgroundJobBase Result { get; set; }
    public virtual ResponseStatus ResponseStatus { get; set; }
}

public partial class GetOpenAiChatStatus
    : IReturn<GetOpenAiChatStatusResponse>, IGet
{
    public virtual long JobId { get; set; }
    public virtual string RefId { get; set; }
}

public partial class GetOpenAiChatStatusResponse
{
    ///<summary>
    ///Unique identifier of the background job
    ///</summary>
    [ApiMember(Description="Unique identifier of the background job")]
    public virtual long JobId { get; set; }

    ///<summary>
    ///Client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Current state of the background job
    ///</summary>
    [ApiMember(Description="Current state of the background job")]
    public virtual BackgroundJobState JobState { get; set; }

    ///<summary>
    ///Current status of the generation request
    ///</summary>
    [ApiMember(Description="Current status of the generation request")]
    public virtual string Status { get; set; }

    ///<summary>
    ///Detailed response status information
    ///</summary>
    [ApiMember(Description="Detailed response status information")]
    public virtual ResponseStatus ResponseStatus { get; set; }

    ///<summary>
    ///Chat result
    ///</summary>
    [ApiMember(Description="Chat result")]
    public virtual OpenAiChatResponse Result { get; set; }
}

public partial class GetSummaryStats
    : IReturn<GetSummaryStatsResponse>, IGet
{
    public virtual DateTime? From { get; set; }
    public virtual DateTime? To { get; set; }
}

public partial class GetSummaryStatsResponse
{
    public virtual List<SummaryStats> ProviderStats { get; set; } = [];
    public virtual List<SummaryStats> ModelStats { get; set; } = [];
    public virtual List<SummaryStats> MonthStats { get; set; } = [];
}

public partial class GetTextGenerationStatus
    : IReturn<GetTextGenerationStatusResponse>, IGet
{
    ///<summary>
    ///Unique identifier of the background job
    ///</summary>
    [ApiMember(Description="Unique identifier of the background job")]
    public virtual long? JobId { get; set; }

    ///<summary>
    ///Client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Client-provided identifier for the request")]
    public virtual string RefId { get; set; }
}

public partial class GetTextGenerationStatusResponse
{
    ///<summary>
    ///Unique identifier of the background job
    ///</summary>
    [ApiMember(Description="Unique identifier of the background job")]
    public virtual long JobId { get; set; }

    ///<summary>
    ///Client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Current state of the background job
    ///</summary>
    [ApiMember(Description="Current state of the background job")]
    public virtual BackgroundJobState JobState { get; set; }

    ///<summary>
    ///Current status of the generation request
    ///</summary>
    [ApiMember(Description="Current status of the generation request")]
    public virtual string Status { get; set; }

    ///<summary>
    ///Generated text
    ///</summary>
    [ApiMember(Description="Generated text")]
    public virtual List<TextOutput> Results { get; set; }

    ///<summary>
    ///Detailed response status information
    ///</summary>
    [ApiMember(Description="Detailed response status information")]
    public virtual ResponseStatus ResponseStatus { get; set; }
}

[Route("/variants/{Variant}/{**Path}")]
public partial class GetVariant
    : IReturn<byte[]>, IGet
{
    [Validate("NotEmpty")]
    public virtual string Variant { get; set; }

    [Validate("NotEmpty")]
    public virtual string Path { get; set; }
}

public partial class GetWorkerStats
    : IReturn<GetWorkerStatsResponse>, IGet
{
}

public partial class GetWorkerStatsResponse
{
    public virtual List<WorkerStats> Results { get; set; } = [];
    public virtual Dictionary<string, int> QueueCounts { get; set; } = new();
    public virtual ResponseStatus ResponseStatus { get; set; }
}

[Route("/hello/{Name}")]
public partial class Hello
    : IReturn<HelloResponse>, IGet
{
    public virtual string Name { get; set; }
}

public partial class HelloResponse
{
    public virtual string Result { get; set; }
}

public partial interface IGeneration
{
    string RefId { get; set; }
    string Tag { get; set; }
}

[DataContract]
public enum ImageOutputFormat
{
    [EnumMember(Value="jpg")]
    Jpg,
    [EnumMember(Value="png")]
    Png,
    [EnumMember(Value="gif")]
    Gif,
    [EnumMember(Value="bmp")]
    Bmp,
    [EnumMember(Value="tiff")]
    Tiff,
    [EnumMember(Value="webp")]
    Webp,
}

///<summary>
///Generate image from another image
///</summary>
[Api(Description="Generate image from another image")]
public partial class ImageToImage
    : IReturn<ArtifactGenerationResponse>, IGeneration
{
    ///<summary>
    ///The image to use as input
    ///</summary>
    [ApiMember(Description="The image to use as input")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Prompt describing the desired output
    ///</summary>
    [ApiMember(Description="Prompt describing the desired output")]
    [Validate("NotEmpty")]
    public virtual string PositivePrompt { get; set; }

    ///<summary>
    ///Negative prompt describing what should not be in the image
    ///</summary>
    [ApiMember(Description="Negative prompt describing what should not be in the image")]
    public virtual string NegativePrompt { get; set; }

    ///<summary>
    ///The AI model to use for image generation
    ///</summary>
    [ApiMember(Description="The AI model to use for image generation")]
    public virtual string Model { get; set; }

    ///<summary>
    ///Optional specific amount of denoise to apply
    ///</summary>
    [ApiMember(Description="Optional specific amount of denoise to apply")]
    public virtual float? Denoise { get; set; }

    ///<summary>
    ///Number of images to generate in a single batch
    ///</summary>
    [ApiMember(Description="Number of images to generate in a single batch")]
    public virtual int? BatchSize { get; set; }

    ///<summary>
    ///Optional seed for reproducible results in image generation
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results in image generation")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Convert image to text
///</summary>
[Api(Description="Convert image to text")]
public partial class ImageToText
    : IReturn<TextGenerationResponse>, IGeneration
{
    ///<summary>
    ///The image to convert to text
    ///</summary>
    [ApiMember(Description="The image to convert to text")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Whether to use a Vision Model for the request
    ///</summary>
    [ApiMember(Description="Whether to use a Vision Model for the request")]
    public virtual string Model { get; set; }

    ///<summary>
    ///Prompt for the vision model
    ///</summary>
    [ApiMember(Description="Prompt for the vision model")]
    public virtual string Prompt { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Upscale an image
///</summary>
[Api(Description="Upscale an image")]
public partial class ImageUpscale
    : IReturn<ArtifactGenerationResponse>, IGeneration
{
    ///<summary>
    ///The image to upscale
    ///</summary>
    [ApiMember(Description="The image to upscale")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Optional seed for reproducible results in image generation
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results in image generation")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Generate image with masked area
///</summary>
[Api(Description="Generate image with masked area")]
public partial class ImageWithMask
    : IReturn<ArtifactGenerationResponse>, IGeneration
{
    ///<summary>
    ///Prompt describing the desired output in the masked area
    ///</summary>
    [ApiMember(Description="Prompt describing the desired output in the masked area")]
    [Validate("NotEmpty")]
    public virtual string PositivePrompt { get; set; }

    ///<summary>
    ///Negative prompt describing what should not be in the masked area
    ///</summary>
    [ApiMember(Description="Negative prompt describing what should not be in the masked area")]
    public virtual string NegativePrompt { get; set; }

    ///<summary>
    ///The image to use as input
    ///</summary>
    [ApiMember(Description="The image to use as input")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///The mask to use as input
    ///</summary>
    [ApiMember(Description="The mask to use as input")]
    [Required]
    public virtual string Mask { get; set; }

    ///<summary>
    ///Optional specific amount of denoise to apply
    ///</summary>
    [ApiMember(Description="Optional specific amount of denoise to apply")]
    public virtual float? Denoise { get; set; }

    ///<summary>
    ///Optional seed for reproducible results in image generation
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results in image generation")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial interface IMediaTransform
{
    string RefId { get; set; }
    string Tag { get; set; }
}

///<summary>
///Base class for queue generation requests
///</summary>
public partial interface IQueueGeneration
{
    string RefId { get; set; }
    string ReplyTo { get; set; }
    string Tag { get; set; }
    string State { get; set; }
}

public partial interface IQueueMediaTransform
{
    string RefId { get; set; }
    string Tag { get; set; }
    string ReplyTo { get; set; }
}

[DataContract]
public enum MediaOutputFormat
{
    [EnumMember(Value="mp4")]
    MP4,
    [EnumMember(Value="avi")]
    AVI,
    [EnumMember(Value="mkv")]
    MKV,
    [EnumMember(Value="mov")]
    MOV,
    [EnumMember(Value="webm")]
    WebM,
    [EnumMember(Value="gif")]
    GIF,
    [EnumMember(Value="mp3")]
    MP3,
    [EnumMember(Value="wav")]
    WAV,
    [EnumMember(Value="flac")]
    FLAC,
}

public partial class MediaTransformArgs
{
    public virtual MediaTransformTaskType? TaskType { get; set; }
    public virtual Stream VideoInput { get; set; }
    public virtual Stream AudioInput { get; set; }
    public virtual Stream ImageInput { get; set; }
    public virtual Stream WatermarkInput { get; set; }
    public virtual string VideoFileName { get; set; }
    public virtual string AudioFileName { get; set; }
    public virtual string ImageFileName { get; set; }
    public virtual string WatermarkFileName { get; set; }
    public virtual MediaOutputFormat? OutputFormat { get; set; }
    public virtual ImageOutputFormat? ImageOutputFormat { get; set; }
    public virtual int? ScaleWidth { get; set; }
    public virtual int? ScaleHeight { get; set; }
    public virtual int? CropX { get; set; }
    public virtual int? CropY { get; set; }
    public virtual int? CropWidth { get; set; }
    public virtual int? CropHeight { get; set; }
    public virtual float? CutStart { get; set; }
    public virtual float? CutEnd { get; set; }
    public virtual Stream WatermarkFile { get; set; }
    public virtual string WatermarkPosition { get; set; }
    public virtual string WatermarkScale { get; set; }
    public virtual string AudioCodec { get; set; }
    public virtual string VideoCodec { get; set; }
    public virtual string AudioBitrate { get; set; }
    public virtual int? AudioSampleRate { get; set; }
}

public enum MediaTransformTaskType
{
    ImageScale,
    VideoScale,
    ImageConvert,
    AudioConvert,
    VideoConvert,
    ImageCrop,
    VideoCrop,
    VideoCut,
    AudioCut,
    WatermarkImage,
    WatermarkVideo,
}

///<summary>
///Generate a response for a given prompt with a provided model.
///</summary>
[Api(Description="Generate a response for a given prompt with a provided model.")]
[DataContract]
public partial class OllamaGenerate
{
    ///<summary>
    ///ID of the model to use. See the model endpoint compatibility table for details on which models work with the Chat API
    ///</summary>
    [DataMember(Name="model")]
    [ApiMember(Description="ID of the model to use. See the model endpoint compatibility table for details on which models work with the Chat API")]
    public virtual string Model { get; set; }

    ///<summary>
    ///The prompt to generate a response for
    ///</summary>
    [DataMember(Name="prompt")]
    [ApiMember(Description="The prompt to generate a response for")]
    public virtual string Prompt { get; set; }

    ///<summary>
    ///The text after the model response
    ///</summary>
    [DataMember(Name="suffix")]
    [ApiMember(Description="The text after the model response")]
    public virtual string Suffix { get; set; }

    ///<summary>
    ///List of base64 images referenced in this request
    ///</summary>
    [DataMember(Name="images")]
    [ApiMember(Description="List of base64 images referenced in this request")]
    public virtual List<string> Images { get; set; }

    ///<summary>
    ///The format to return a response in. Format can be `json` or a JSON schema
    ///</summary>
    [DataMember(Name="format")]
    [ApiMember(Description="The format to return a response in. Format can be `json` or a JSON schema")]
    public virtual string Format { get; set; }

    ///<summary>
    ///Additional model parameters
    ///</summary>
    [DataMember(Name="options")]
    [ApiMember(Description="Additional model parameters")]
    public virtual OllamaGenerateOptions Options { get; set; }

    ///<summary>
    ///System message
    ///</summary>
    [DataMember(Name="system")]
    [ApiMember(Description="System message")]
    public virtual string System { get; set; }

    ///<summary>
    ///The prompt template to use
    ///</summary>
    [DataMember(Name="template")]
    [ApiMember(Description="The prompt template to use")]
    public virtual string Template { get; set; }

    ///<summary>
    ///If set, partial message deltas will be sent, like in ChatGPT. Tokens will be sent as data-only server-sent events as they become available, with the stream terminated by a `data: [DONE]` message
    ///</summary>
    [DataMember(Name="stream")]
    [ApiMember(Description="If set, partial message deltas will be sent, like in ChatGPT. Tokens will be sent as data-only server-sent events as they become available, with the stream terminated by a `data: [DONE]` message")]
    public virtual bool? Stream { get; set; }

    ///<summary>
    ///If `true` no formatting will be applied to the prompt. You may choose to use the raw parameter if you are specifying a full templated prompt in your request to the API
    ///</summary>
    [DataMember(Name="raw")]
    [ApiMember(Description="If `true` no formatting will be applied to the prompt. You may choose to use the raw parameter if you are specifying a full templated prompt in your request to the API")]
    public virtual bool? Raw { get; set; }

    ///<summary>
    ///Controls how long the model will stay loaded into memory following the request (default: 5m)
    ///</summary>
    [DataMember(Name="keep_alive")]
    [ApiMember(Description="Controls how long the model will stay loaded into memory following the request (default: 5m)")]
    public virtual string keep_alive { get; set; }

    ///<summary>
    ///The context parameter returned from a previous request to /generate, this can be used to keep a short conversational memory
    ///</summary>
    [DataMember(Name="context")]
    [ApiMember(Description="The context parameter returned from a previous request to /generate, this can be used to keep a short conversational memory")]
    public virtual List<int> Context { get; set; }
}

public partial class OllamaGenerateOptions
{
    ///<summary>
    ///Enable Mirostat sampling for controlling perplexity. (default: 0, 0 = disabled, 1 = Mirostat, 2 = Mirostat 2.0)
    ///</summary>
    [DataMember(Name="mirostat")]
    [ApiMember(Description="Enable Mirostat sampling for controlling perplexity. (default: 0, 0 = disabled, 1 = Mirostat, 2 = Mirostat 2.0)")]
    public virtual int? Mirostat { get; set; }

    ///<summary>
    ///Influences how quickly the algorithm responds to feedback from the generated text. A lower learning rate will result in slower adjustments, while a higher learning rate will make the algorithm more responsive. (Default: 0.1)
    ///</summary>
    [DataMember(Name="mirostat_eta")]
    [ApiMember(Description="Influences how quickly the algorithm responds to feedback from the generated text. A lower learning rate will result in slower adjustments, while a higher learning rate will make the algorithm more responsive. (Default: 0.1)")]
    public virtual double? MirostatEta { get; set; }

    ///<summary>
    ///Controls the balance between coherence and diversity of the output. A lower value will result in more focused and coherent text. (Default: 5.0)
    ///</summary>
    [DataMember(Name="mirostat_tau")]
    [ApiMember(Description="Controls the balance between coherence and diversity of the output. A lower value will result in more focused and coherent text. (Default: 5.0)")]
    public virtual double? MirostatTau { get; set; }

    ///<summary>
    ///Sets the size of the context window used to generate the next token. (Default: 2048)
    ///</summary>
    [DataMember(Name="num_ctx")]
    [ApiMember(Description="Sets the size of the context window used to generate the next token. (Default: 2048)")]
    public virtual int? NumCtx { get; set; }

    ///<summary>
    ///Sets how far back for the model to look back to prevent repetition. (Default: 64, 0 = disabled, -1 = num_ctx)
    ///</summary>
    [DataMember(Name="repeat_last_n")]
    [ApiMember(Description="Sets how far back for the model to look back to prevent repetition. (Default: 64, 0 = disabled, -1 = num_ctx)")]
    public virtual int? RepeatLastN { get; set; }

    ///<summary>
    ///Sets how strongly to penalize repetitions. A higher value (e.g., 1.5) will penalize repetitions more strongly, while a lower value (e.g., 0.9) will be more lenient. (Default: 1.1)
    ///</summary>
    [DataMember(Name="repeat_penalty")]
    [ApiMember(Description="Sets how strongly to penalize repetitions. A higher value (e.g., 1.5) will penalize repetitions more strongly, while a lower value (e.g., 0.9) will be more lenient. (Default: 1.1)")]
    public virtual double? RepeatPenalty { get; set; }

    ///<summary>
    ///The temperature of the model. Increasing the temperature will make the model answer more creatively. (Default: 0.8)
    ///</summary>
    [DataMember(Name="temperature")]
    [ApiMember(Description="The temperature of the model. Increasing the temperature will make the model answer more creatively. (Default: 0.8)")]
    public virtual double? Temperature { get; set; }

    ///<summary>
    ///Sets the random number seed to use for generation. Setting this to a specific number will make the model generate the same text for the same prompt. (Default: 0)
    ///</summary>
    [DataMember(Name="seed")]
    [ApiMember(Description="Sets the random number seed to use for generation. Setting this to a specific number will make the model generate the same text for the same prompt. (Default: 0)")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Sets the stop sequences to use. When this pattern is encountered the LLM will stop generating text and return. Multiple stop patterns may be set by specifying multiple separate stop parameters in a modelfile.	
    ///</summary>
    [DataMember(Name="stop")]
    [ApiMember(Description="Sets the stop sequences to use. When this pattern is encountered the LLM will stop generating text and return. Multiple stop patterns may be set by specifying multiple separate stop parameters in a modelfile.\t")]
    public virtual string Stop { get; set; }

    ///<summary>
    ///Maximum number of tokens to predict when generating text. (Default: -1, infinite generation)
    ///</summary>
    [DataMember(Name="num_predict")]
    [ApiMember(Description="Maximum number of tokens to predict when generating text. (Default: -1, infinite generation)")]
    public virtual int? NumPredict { get; set; }

    ///<summary>
    ///Reduces the probability of generating nonsense. A higher value (e.g. 100) will give more diverse answers, while a lower value (e.g. 10) will be more conservative. (Default: 40)
    ///</summary>
    [DataMember(Name="top_k")]
    [ApiMember(Description="Reduces the probability of generating nonsense. A higher value (e.g. 100) will give more diverse answers, while a lower value (e.g. 10) will be more conservative. (Default: 40)")]
    public virtual int? TopK { get; set; }

    ///<summary>
    ///Works together with top-k. A higher value (e.g., 0.95) will lead to more diverse text, while a lower value (e.g., 0.5) will generate more focused and conservative text. (Default: 0.9)
    ///</summary>
    [DataMember(Name="top_p")]
    [ApiMember(Description="Works together with top-k. A higher value (e.g., 0.95) will lead to more diverse text, while a lower value (e.g., 0.5) will generate more focused and conservative text. (Default: 0.9)")]
    public virtual double? TopP { get; set; }

    ///<summary>
    ///Alternative to the top_p, and aims to ensure a balance of quality and variety. The parameter p represents the minimum probability for a token to be considered, relative to the probability of the most likely token. For example, with p=0.05 and the most likely token having a probability of 0.9, logits with a value less than 0.045 are filtered out. (Default: 0.0)
    ///</summary>
    [DataMember(Name="min_p")]
    [ApiMember(Description="Alternative to the top_p, and aims to ensure a balance of quality and variety. The parameter p represents the minimum probability for a token to be considered, relative to the probability of the most likely token. For example, with p=0.05 and the most likely token having a probability of 0.9, logits with a value less than 0.045 are filtered out. (Default: 0.0)")]
    public virtual double? MinP { get; set; }
}

[DataContract]
public partial class OllamaGenerateResponse
{
    ///<summary>
    ///The model used
    ///</summary>
    [DataMember(Name="model")]
    [ApiMember(Description="The model used")]
    public virtual string Model { get; set; }

    ///<summary>
    ///The Unix timestamp (in seconds) of when the chat completion was created.
    ///</summary>
    [DataMember(Name="created_at")]
    [ApiMember(Description="The Unix timestamp (in seconds) of when the chat completion was created.")]
    public virtual long CreatedAt { get; set; }

    ///<summary>
    ///The full response
    ///</summary>
    [DataMember(Name="response")]
    [ApiMember(Description="The full response")]
    public virtual string Response { get; set; }

    ///<summary>
    ///Whether the response is done
    ///</summary>
    [DataMember(Name="done")]
    [ApiMember(Description="Whether the response is done")]
    public virtual bool Done { get; set; }

    ///<summary>
    ///The reason the response completed
    ///</summary>
    [DataMember(Name="done_reason")]
    [ApiMember(Description="The reason the response completed")]
    public virtual string DoneReason { get; set; }

    ///<summary>
    ///Time spent generating the response
    ///</summary>
    [DataMember(Name="total_duration")]
    [ApiMember(Description="Time spent generating the response")]
    public virtual int TotalDuration { get; set; }

    ///<summary>
    ///Time spent in nanoseconds loading the model
    ///</summary>
    [DataMember(Name="load_duration")]
    [ApiMember(Description="Time spent in nanoseconds loading the model")]
    public virtual int LoadDuration { get; set; }

    ///<summary>
    ///Time spent in nanoseconds evaluating the prompt
    ///</summary>
    [DataMember(Name="prompt_eval_count")]
    [ApiMember(Description="Time spent in nanoseconds evaluating the prompt")]
    public virtual int PromptEvalCount { get; set; }

    ///<summary>
    ///Time spent in nanoseconds evaluating the prompt
    ///</summary>
    [DataMember(Name="prompt_eval_duration")]
    [ApiMember(Description="Time spent in nanoseconds evaluating the prompt")]
    public virtual int PromptEvalDuration { get; set; }

    ///<summary>
    ///Number of tokens in the response
    ///</summary>
    [DataMember(Name="eval_count")]
    [ApiMember(Description="Number of tokens in the response")]
    public virtual int EvalCount { get; set; }

    ///<summary>
    ///Time in nanoseconds spent generating the response
    ///</summary>
    [DataMember(Name="prompt_tokens")]
    [ApiMember(Description="Time in nanoseconds spent generating the response")]
    public virtual int PromptTokens { get; set; }

    ///<summary>
    ///An encoding of the conversation used in this response, this can be sent in the next request to keep a conversational memory
    ///</summary>
    [DataMember(Name="context")]
    [ApiMember(Description="An encoding of the conversation used in this response, this can be sent in the next request to keep a conversational memory")]
    public virtual List<int> Context { get; set; }

    [DataMember(Name="responseStatus")]
    public virtual ResponseStatus ResponseStatus { get; set; }
}

///<summary>
///Generate a response for a given prompt with a provided model.
///</summary>
[Route("/api/generate", "POST")]
[Api(Description="Generate a response for a given prompt with a provided model.")]
public partial class OllamaGeneration
    : OllamaGenerate, IReturn<OllamaGenerateResponse>, IPost
{
    ///<summary>
    ///Provide a unique identifier to track requests
    ///</summary>
    [ApiMember(Description="Provide a unique identifier to track requests")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Specify which AI Provider to use
    ///</summary>
    [ApiMember(Description="Specify which AI Provider to use")]
    public virtual string Provider { get; set; }

    ///<summary>
    ///Categorize like requests under a common group
    ///</summary>
    [ApiMember(Description="Categorize like requests under a common group")]
    public virtual string Tag { get; set; }
}

[DataContract]
public partial class OllamaModel
{
    [DataMember(Name="name")]
    public virtual string Name { get; set; }

    [DataMember(Name="model")]
    public virtual string Model { get; set; }

    [DataMember(Name="modified_at")]
    public virtual DateTime ModifiedAt { get; set; }

    [DataMember(Name="size")]
    public virtual long Size { get; set; }

    [DataMember(Name="digest")]
    public virtual string Digest { get; set; }

    [DataMember(Name="details")]
    public virtual OllamaModelDetails Details { get; set; }
}

[DataContract]
public partial class OllamaModelDetails
{
    [DataMember(Name="parent_model")]
    public virtual string ParentModel { get; set; }

    [DataMember(Name="format")]
    public virtual string Format { get; set; }

    [DataMember(Name="family")]
    public virtual string Family { get; set; }

    [DataMember(Name="families")]
    public virtual List<string> Families { get; set; } = [];

    [DataMember(Name="parameter_size")]
    public virtual string ParameterSize { get; set; }

    [DataMember(Name="quantization_level")]
    public virtual string QuantizationLevel { get; set; }
}

///<summary>
///Given a list of messages comprising a conversation, the model will return a response.
///</summary>
[Api(Description="Given a list of messages comprising a conversation, the model will return a response.")]
[DataContract]
public partial class OpenAiChat
{
    ///<summary>
    ///A list of messages comprising the conversation so far.
    ///</summary>
    [DataMember(Name="messages")]
    [ApiMember(Description="A list of messages comprising the conversation so far.")]
    public virtual List<OpenAiMessage> Messages { get; set; } = [];

    ///<summary>
    ///ID of the model to use. See the model endpoint compatibility table for details on which models work with the Chat API
    ///</summary>
    [DataMember(Name="model")]
    [ApiMember(Description="ID of the model to use. See the model endpoint compatibility table for details on which models work with the Chat API")]
    public virtual string Model { get; set; }

    ///<summary>
    ///Number between `-2.0` and `2.0`. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
    ///</summary>
    [DataMember(Name="frequency_penalty")]
    [ApiMember(Description="Number between `-2.0` and `2.0`. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.")]
    public virtual double? FrequencyPenalty { get; set; }

    ///<summary>
    ///Modify the likelihood of specified tokens appearing in the completion.
    ///</summary>
    [DataMember(Name="logit_bias")]
    [ApiMember(Description="Modify the likelihood of specified tokens appearing in the completion.")]
    public virtual Dictionary<int, int> LogitBias { get; set; }

    ///<summary>
    ///Whether to return log probabilities of the output tokens or not. If true, returns the log probabilities of each output token returned in the content of message.
    ///</summary>
    [DataMember(Name="logprobs")]
    [ApiMember(Description="Whether to return log probabilities of the output tokens or not. If true, returns the log probabilities of each output token returned in the content of message.")]
    public virtual bool? LogProbs { get; set; }

    ///<summary>
    ///An integer between 0 and 20 specifying the number of most likely tokens to return at each token position, each with an associated log probability. logprobs must be set to true if this parameter is used.
    ///</summary>
    [DataMember(Name="top_logprobs")]
    [ApiMember(Description="An integer between 0 and 20 specifying the number of most likely tokens to return at each token position, each with an associated log probability. logprobs must be set to true if this parameter is used.")]
    public virtual int? TopLogProbs { get; set; }

    ///<summary>
    ///The maximum number of tokens that can be generated in the chat completion.
    ///</summary>
    [DataMember(Name="max_tokens")]
    [ApiMember(Description="The maximum number of tokens that can be generated in the chat completion.")]
    public virtual int? MaxTokens { get; set; }

    ///<summary>
    ///How many chat completion choices to generate for each input message. Note that you will be charged based on the number of generated tokens across all of the choices. Keep `n` as `1` to minimize costs.
    ///</summary>
    [DataMember(Name="n")]
    [ApiMember(Description="How many chat completion choices to generate for each input message. Note that you will be charged based on the number of generated tokens across all of the choices. Keep `n` as `1` to minimize costs.")]
    public virtual int? N { get; set; }

    ///<summary>
    ///Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.
    ///</summary>
    [DataMember(Name="presence_penalty")]
    [ApiMember(Description="Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.")]
    public virtual double? PresencePenalty { get; set; }

    ///<summary>
    ///An object specifying the format that the model must output. Compatible with GPT-4 Turbo and all GPT-3.5 Turbo models newer than `gpt-3.5-turbo-1106`. Setting Type to ResponseFormat.JsonObject enables JSON mode, which guarantees the message the model generates is valid JSON.
    ///</summary>
    [DataMember(Name="response_format")]
    [ApiMember(Description="An object specifying the format that the model must output. Compatible with GPT-4 Turbo and all GPT-3.5 Turbo models newer than `gpt-3.5-turbo-1106`. Setting Type to ResponseFormat.JsonObject enables JSON mode, which guarantees the message the model generates is valid JSON.")]
    public virtual OpenAiResponseFormat ResponseFormat { get; set; }

    ///<summary>
    ///This feature is in Beta. If specified, our system will make a best effort to sample deterministically, such that repeated requests with the same seed and parameters should return the same result. Determinism is not guaranteed, and you should refer to the system_fingerprint response parameter to monitor changes in the backend.
    ///</summary>
    [DataMember(Name="seed")]
    [ApiMember(Description="This feature is in Beta. If specified, our system will make a best effort to sample deterministically, such that repeated requests with the same seed and parameters should return the same result. Determinism is not guaranteed, and you should refer to the system_fingerprint response parameter to monitor changes in the backend.")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Up to 4 sequences where the API will stop generating further tokens.
    ///</summary>
    [DataMember(Name="stop")]
    [ApiMember(Description="Up to 4 sequences where the API will stop generating further tokens.")]
    public virtual List<string> Stop { get; set; }

    ///<summary>
    ///If set, partial message deltas will be sent, like in ChatGPT. Tokens will be sent as data-only server-sent events as they become available, with the stream terminated by a `data: [DONE]` message.
    ///</summary>
    [DataMember(Name="stream")]
    [ApiMember(Description="If set, partial message deltas will be sent, like in ChatGPT. Tokens will be sent as data-only server-sent events as they become available, with the stream terminated by a `data: [DONE]` message.")]
    public virtual bool? Stream { get; set; }

    ///<summary>
    ///What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.
    ///</summary>
    [DataMember(Name="temperature")]
    [ApiMember(Description="What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.")]
    public virtual double? Temperature { get; set; }

    ///<summary>
    ///An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered.
    ///</summary>
    [DataMember(Name="top_p")]
    [ApiMember(Description="An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered.")]
    public virtual double? TopP { get; set; }

    ///<summary>
    ///A list of tools the model may call. Currently, only functions are supported as a tool. Use this to provide a list of functions the model may generate JSON inputs for. A max of 128 functions are supported.
    ///</summary>
    [DataMember(Name="tools")]
    [ApiMember(Description="A list of tools the model may call. Currently, only functions are supported as a tool. Use this to provide a list of functions the model may generate JSON inputs for. A max of 128 functions are supported.")]
    public virtual List<OpenAiTools> Tools { get; set; }

    ///<summary>
    ///A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
    ///</summary>
    [DataMember(Name="user")]
    [ApiMember(Description="A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.")]
    public virtual string User { get; set; }
}

///<summary>
///Given a list of messages comprising a conversation, the model will return a response.
///</summary>
[Route("/v1/chat/completions", "POST")]
[Api(Description="Given a list of messages comprising a conversation, the model will return a response.")]
public partial class OpenAiChatCompletion
    : OpenAiChat, IReturn<OpenAiChatResponse>, IPost
{
    ///<summary>
    ///Provide a unique identifier to track requests
    ///</summary>
    [ApiMember(Description="Provide a unique identifier to track requests")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Specify which AI Provider to use
    ///</summary>
    [ApiMember(Description="Specify which AI Provider to use")]
    public virtual string Provider { get; set; }

    ///<summary>
    ///Categorize like requests under a common group
    ///</summary>
    [ApiMember(Description="Categorize like requests under a common group")]
    public virtual string Tag { get; set; }
}

[DataContract]
public partial class OpenAiChatResponse
{
    ///<summary>
    ///A unique identifier for the chat completion.
    ///</summary>
    [DataMember(Name="id")]
    [ApiMember(Description="A unique identifier for the chat completion.")]
    public virtual string Id { get; set; }

    ///<summary>
    ///A list of chat completion choices. Can be more than one if n is greater than 1.
    ///</summary>
    [DataMember(Name="choices")]
    [ApiMember(Description="A list of chat completion choices. Can be more than one if n is greater than 1.")]
    public virtual List<Choice> Choices { get; set; } = [];

    ///<summary>
    ///The Unix timestamp (in seconds) of when the chat completion was created.
    ///</summary>
    [DataMember(Name="created")]
    [ApiMember(Description="The Unix timestamp (in seconds) of when the chat completion was created.")]
    public virtual long Created { get; set; }

    ///<summary>
    ///The model used for the chat completion.
    ///</summary>
    [DataMember(Name="model")]
    [ApiMember(Description="The model used for the chat completion.")]
    public virtual string Model { get; set; }

    ///<summary>
    ///This fingerprint represents the backend configuration that the model runs with.
    ///</summary>
    [DataMember(Name="system_fingerprint")]
    [ApiMember(Description="This fingerprint represents the backend configuration that the model runs with.")]
    public virtual string SystemFingerprint { get; set; }

    ///<summary>
    ///The object type, which is always chat.completion.
    ///</summary>
    [DataMember(Name="object")]
    [ApiMember(Description="The object type, which is always chat.completion.")]
    public virtual string Object { get; set; }

    ///<summary>
    ///Usage statistics for the completion request.
    ///</summary>
    [DataMember(Name="usage")]
    [ApiMember(Description="Usage statistics for the completion request.")]
    public virtual OpenAiUsage Usage { get; set; }

    [DataMember(Name="responseStatus")]
    public virtual ResponseStatus ResponseStatus { get; set; }
}

///<summary>
///A list of messages comprising the conversation so far.
///</summary>
[Api(Description="A list of messages comprising the conversation so far.")]
[DataContract]
public partial class OpenAiMessage
{
    ///<summary>
    ///The contents of the message.
    ///</summary>
    [DataMember(Name="content")]
    [ApiMember(Description="The contents of the message.")]
    public virtual string Content { get; set; }

    ///<summary>
    ///The images for the message.
    ///</summary>
    [DataMember(Name="images")]
    [ApiMember(Description="The images for the message.")]
    public virtual List<string> Images { get; set; } = [];

    ///<summary>
    ///The role of the author of this message. Valid values are `system`, `user`, `assistant` and `tool`.
    ///</summary>
    [DataMember(Name="role")]
    [ApiMember(Description="The role of the author of this message. Valid values are `system`, `user`, `assistant` and `tool`.")]
    public virtual string Role { get; set; }

    ///<summary>
    ///An optional name for the participant. Provides the model information to differentiate between participants of the same role.
    ///</summary>
    [DataMember(Name="name")]
    [ApiMember(Description="An optional name for the participant. Provides the model information to differentiate between participants of the same role.")]
    public virtual string Name { get; set; }

    ///<summary>
    ///The tool calls generated by the model, such as function calls.
    ///</summary>
    [DataMember(Name="tool_calls")]
    [ApiMember(Description="The tool calls generated by the model, such as function calls.")]
    public virtual List<ToolCall> ToolCalls { get; set; }

    ///<summary>
    ///Tool call that this message is responding to.
    ///</summary>
    [DataMember(Name="tool_call_id")]
    [ApiMember(Description="Tool call that this message is responding to.")]
    public virtual string ToolCallId { get; set; }
}

[DataContract]
public partial class OpenAiResponseFormat
{
    ///<summary>
    ///An object specifying the format that the model must output. Compatible with GPT-4 Turbo and all GPT-3.5 Turbo models newer than gpt-3.5-turbo-1106.
    ///</summary>
    [DataMember(Name="response_format")]
    [ApiMember(Description="An object specifying the format that the model must output. Compatible with GPT-4 Turbo and all GPT-3.5 Turbo models newer than gpt-3.5-turbo-1106.")]
    public virtual ResponseFormat Type { get; set; }
}

[DataContract]
public partial class OpenAiTools
{
    ///<summary>
    ///The type of the tool. Currently, only function is supported.
    ///</summary>
    [DataMember(Name="type")]
    [ApiMember(Description="The type of the tool. Currently, only function is supported.")]
    public virtual OpenAiToolType Type { get; set; }
}

public enum OpenAiToolType
{
    [EnumMember(Value="function")]
    Function,
}

///<summary>
///Usage statistics for the completion request.
///</summary>
[Api(Description="Usage statistics for the completion request.")]
[DataContract]
public partial class OpenAiUsage
{
    ///<summary>
    ///Number of tokens in the generated completion.
    ///</summary>
    [DataMember(Name="completion_tokens")]
    [ApiMember(Description="Number of tokens in the generated completion.")]
    public virtual int CompletionTokens { get; set; }

    ///<summary>
    ///Number of tokens in the prompt.
    ///</summary>
    [DataMember(Name="prompt_tokens")]
    [ApiMember(Description="Number of tokens in the prompt.")]
    public virtual int PromptTokens { get; set; }

    ///<summary>
    ///Total number of tokens used in the request (prompt + completion).
    ///</summary>
    [DataMember(Name="total_tokens")]
    [ApiMember(Description="Total number of tokens used in the request (prompt + completion).")]
    public virtual int TotalTokens { get; set; }
}

public partial class PageStats
{
    public virtual string Label { get; set; }
    public virtual int Total { get; set; }
}

///<summary>
///Different Models available in AI Server
///</summary>
[Api(Description="Different Models available in AI Server")]
public partial class QueryAiModels
    : QueryDb<AiModel>, IReturn<QueryResponse<AiModel>>
{
}

///<summary>
///AI Providers
///</summary>
[Api(Description="AI Providers")]
public partial class QueryAiProviders
    : QueryDb<AiProvider>, IReturn<QueryResponse<AiProvider>>
{
    public virtual string Name { get; set; }
}

///<summary>
///The Type and behavior of different API Providers
///</summary>
[Api(Description="The Type and behavior of different API Providers")]
public partial class QueryAiTypes
    : QueryDb<AiType>, IReturn<QueryResponse<AiType>>
{
}

///<summary>
///Media Models
///</summary>
[Api(Description="Media Models")]
public partial class QueryMediaModels
    : QueryDb<MediaModel>, IReturn<QueryResponse<MediaModel>>
{
    public virtual string Id { get; set; }
    public virtual string ProviderId { get; set; }
}

public partial class QueryMediaProviders
    : QueryDb<MediaProvider>, IReturn<QueryResponse<MediaProvider>>
{
    public virtual int? Id { get; set; }
    public virtual string Name { get; set; }
}

///<summary>
///Text to Speech Voice models
///</summary>
[Api(Description="Text to Speech Voice models")]
public partial class QueryTextToSpeechVoices
    : QueryDb<TextToSpeechVoice>, IReturn<QueryResponse<TextToSpeechVoice>>
{
}

public partial class QueueConvertAudio
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform
{
    ///<summary>
    ///The desired output format for the converted audio
    ///</summary>
    [ApiMember(Description="The desired output format for the converted audio")]
    [Required]
    public virtual AudioFormat OutputFormat { get; set; }

    [Required]
    public virtual string Audio { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial class QueueConvertImage
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform, IPost
{
    ///<summary>
    ///The image file to be converted
    ///</summary>
    [ApiMember(Description="The image file to be converted")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///The desired output format for the converted image
    ///</summary>
    [ApiMember(Description="The desired output format for the converted image")]
    [Required]
    public virtual ImageOutputFormat? OutputFormat { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial class QueueConvertVideo
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform
{
    ///<summary>
    ///The desired output format for the converted video
    ///</summary>
    [ApiMember(Description="The desired output format for the converted video")]
    [Required]
    public virtual ConvertVideoOutputFormat OutputFormat { get; set; }

    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial class QueueCropImage
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform, IPost
{
    ///<summary>
    ///The X-coordinate of the top-left corner of the crop area
    ///</summary>
    [ApiMember(Description="The X-coordinate of the top-left corner of the crop area")]
    public virtual int X { get; set; }

    ///<summary>
    ///The Y-coordinate of the top-left corner of the crop area
    ///</summary>
    [ApiMember(Description="The Y-coordinate of the top-left corner of the crop area")]
    public virtual int Y { get; set; }

    ///<summary>
    ///The width of the crop area
    ///</summary>
    [ApiMember(Description="The width of the crop area")]
    public virtual int Width { get; set; }

    ///<summary>
    ///The height of the crop area
    ///</summary>
    [ApiMember(Description="The height of the crop area")]
    public virtual int Height { get; set; }

    ///<summary>
    ///The image file to be cropped
    ///</summary>
    [ApiMember(Description="The image file to be cropped")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial class QueueCropVideo
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform
{
    ///<summary>
    ///The X-coordinate of the top-left corner of the crop area
    ///</summary>
    [ApiMember(Description="The X-coordinate of the top-left corner of the crop area")]
    [Validate("GreaterThan(0)")]
    [Required]
    public virtual int X { get; set; }

    ///<summary>
    ///The Y-coordinate of the top-left corner of the crop area
    ///</summary>
    [ApiMember(Description="The Y-coordinate of the top-left corner of the crop area")]
    [Validate("GreaterThan(0)")]
    [Required]
    public virtual int Y { get; set; }

    ///<summary>
    ///The width of the crop area
    ///</summary>
    [ApiMember(Description="The width of the crop area")]
    [Validate("GreaterThan(0)")]
    [Required]
    public virtual int Width { get; set; }

    ///<summary>
    ///The height of the crop area
    ///</summary>
    [ApiMember(Description="The height of the crop area")]
    [Validate("GreaterThan(0)")]
    [Required]
    public virtual int Height { get; set; }

    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial class QueueGenerationResponse
{
    ///<summary>
    ///Unique identifier of the background job
    ///</summary>
    [ApiMember(Description="Unique identifier of the background job")]
    public virtual long JobId { get; set; }

    ///<summary>
    ///Client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Current state of the background job
    ///</summary>
    [ApiMember(Description="Current state of the background job")]
    public virtual BackgroundJobState JobState { get; set; }

    ///<summary>
    ///Current status of the generation request
    ///</summary>
    [ApiMember(Description="Current status of the generation request")]
    public virtual string Status { get; set; }

    ///<summary>
    ///Detailed response status information
    ///</summary>
    [ApiMember(Description="Detailed response status information")]
    public virtual ResponseStatus ResponseStatus { get; set; }

    ///<summary>
    ///URL to check the status of the generation request
    ///</summary>
    [ApiMember(Description="URL to check the status of the generation request")]
    public virtual string StatusUrl { get; set; }
}

///<summary>
///Generate image from another image
///</summary>
[Api(Description="Generate image from another image")]
public partial class QueueImageToImage
    : IReturn<QueueGenerationResponse>, IQueueGeneration
{
    ///<summary>
    ///The image to use as input
    ///</summary>
    [ApiMember(Description="The image to use as input")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Prompt describing the desired output
    ///</summary>
    [ApiMember(Description="Prompt describing the desired output")]
    [Validate("NotEmpty")]
    public virtual string PositivePrompt { get; set; }

    ///<summary>
    ///Negative prompt describing what should not be in the image
    ///</summary>
    [ApiMember(Description="Negative prompt describing what should not be in the image")]
    public virtual string NegativePrompt { get; set; }

    ///<summary>
    ///The AI model to use for image generation
    ///</summary>
    [ApiMember(Description="The AI model to use for image generation")]
    public virtual string Model { get; set; }

    ///<summary>
    ///Optional specific amount of denoise to apply
    ///</summary>
    [ApiMember(Description="Optional specific amount of denoise to apply")]
    public virtual float? Denoise { get; set; }

    ///<summary>
    ///Number of images to generate in a single batch
    ///</summary>
    [ApiMember(Description="Number of images to generate in a single batch")]
    public virtual int? BatchSize { get; set; }

    ///<summary>
    ///Optional seed for reproducible results in image generation
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results in image generation")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Optional state to associate with the request
    ///</summary>
    [ApiMember(Description="Optional state to associate with the request")]
    public virtual string State { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Convert image to text
///</summary>
[Api(Description="Convert image to text")]
public partial class QueueImageToText
    : IReturn<QueueGenerationResponse>, IQueueGeneration
{
    ///<summary>
    ///The image to convert to text
    ///</summary>
    [ApiMember(Description="The image to convert to text")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }

    ///<summary>
    ///Optional state to associate with the request
    ///</summary>
    [ApiMember(Description="Optional state to associate with the request")]
    public virtual string State { get; set; }
}

///<summary>
///Upscale an image
///</summary>
[Api(Description="Upscale an image")]
public partial class QueueImageUpscale
    : IReturn<QueueGenerationResponse>, IQueueGeneration
{
    ///<summary>
    ///The image to upscale
    ///</summary>
    [ApiMember(Description="The image to upscale")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Optional seed for reproducible results in image generation
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results in image generation")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }

    ///<summary>
    ///Optional state to associate with the request
    ///</summary>
    [ApiMember(Description="Optional state to associate with the request")]
    public virtual string State { get; set; }
}

///<summary>
///Generate image with masked area
///</summary>
[Api(Description="Generate image with masked area")]
public partial class QueueImageWithMask
    : IReturn<QueueGenerationResponse>, IQueueGeneration
{
    ///<summary>
    ///Prompt describing the desired output in the masked area
    ///</summary>
    [ApiMember(Description="Prompt describing the desired output in the masked area")]
    [Validate("NotEmpty")]
    public virtual string PositivePrompt { get; set; }

    ///<summary>
    ///Negative prompt describing what should not be in the masked area
    ///</summary>
    [ApiMember(Description="Negative prompt describing what should not be in the masked area")]
    public virtual string NegativePrompt { get; set; }

    ///<summary>
    ///The image to use as input
    ///</summary>
    [ApiMember(Description="The image to use as input")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///The mask to use as input
    ///</summary>
    [ApiMember(Description="The mask to use as input")]
    [Required]
    public virtual string Mask { get; set; }

    ///<summary>
    ///Optional specific amount of denoise to apply
    ///</summary>
    [ApiMember(Description="Optional specific amount of denoise to apply")]
    public virtual float? Denoise { get; set; }

    ///<summary>
    ///Optional seed for reproducible results in image generation
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results in image generation")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }

    ///<summary>
    ///Optional state to associate with the request
    ///</summary>
    [ApiMember(Description="Optional state to associate with the request")]
    public virtual string State { get; set; }
}

public partial class QueueMediaTransformResponse
{
    ///<summary>
    ///Unique identifier of the background job
    ///</summary>
    [ApiMember(Description="Unique identifier of the background job")]
    public virtual long JobId { get; set; }

    ///<summary>
    ///Client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Current state of the background job
    ///</summary>
    [ApiMember(Description="Current state of the background job")]
    public virtual BackgroundJobState JobState { get; set; }

    ///<summary>
    ///Current status of the transformation request
    ///</summary>
    [ApiMember(Description="Current status of the transformation request")]
    public virtual string Status { get; set; }

    ///<summary>
    ///Detailed response status information
    ///</summary>
    [ApiMember(Description="Detailed response status information")]
    public virtual ResponseStatus ResponseStatus { get; set; }

    ///<summary>
    ///URL to check the status of the request
    ///</summary>
    [ApiMember(Description="URL to check the status of the request")]
    public virtual string StatusUrl { get; set; }
}

public partial class QueueOllamaGeneration
    : IReturn<QueueOllamaGenerationResponse>
{
    public virtual string RefId { get; set; }
    public virtual string Provider { get; set; }
    public virtual string ReplyTo { get; set; }
    public virtual string Tag { get; set; }
    public virtual OllamaGenerate Request { get; set; }
}

public partial class QueueOllamaGenerationResponse
{
    public virtual long Id { get; set; }
    public virtual string RefId { get; set; }
    public virtual string StatusUrl { get; set; }
    public virtual ResponseStatus ResponseStatus { get; set; }
}

public partial class QueueOpenAiChatCompletion
    : IReturn<QueueOpenAiChatResponse>
{
    public virtual string RefId { get; set; }
    public virtual string Provider { get; set; }
    public virtual string ReplyTo { get; set; }
    public virtual string Tag { get; set; }
    public virtual OpenAiChat Request { get; set; }
}

public partial class QueueOpenAiChatResponse
{
    public virtual long Id { get; set; }
    public virtual string RefId { get; set; }
    public virtual string StatusUrl { get; set; }
    public virtual ResponseStatus ResponseStatus { get; set; }
}

public partial class QueueScaleImage
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform, IPost
{
    ///<summary>
    ///The image file to be scaled
    ///</summary>
    [ApiMember(Description="The image file to be scaled")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Desired width of the scaled image
    ///</summary>
    [ApiMember(Description="Desired width of the scaled image")]
    public virtual int? Width { get; set; }

    ///<summary>
    ///Desired height of the scaled image
    ///</summary>
    [ApiMember(Description="Desired height of the scaled image")]
    public virtual int? Height { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Scale video
///</summary>
[Api(Description="Scale video")]
public partial class QueueScaleVideo
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform
{
    ///<summary>
    ///The video file to be scaled
    ///</summary>
    [ApiMember(Description="The video file to be scaled")]
    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///Desired width of the scaled video
    ///</summary>
    [ApiMember(Description="Desired width of the scaled video")]
    public virtual int? Width { get; set; }

    ///<summary>
    ///Desired height of the scaled video
    ///</summary>
    [ApiMember(Description="Desired height of the scaled video")]
    public virtual int? Height { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Convert speech to text
///</summary>
[Api(Description="Convert speech to text")]
public partial class QueueSpeechToText
    : IReturn<QueueGenerationResponse>, IQueueGeneration
{
    ///<summary>
    ///The audio stream containing the speech to be transcribed
    ///</summary>
    [ApiMember(Description="The audio stream containing the speech to be transcribed")]
    [Required]
    public virtual string Audio { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }

    ///<summary>
    ///Optional state to associate with the request
    ///</summary>
    [ApiMember(Description="Optional state to associate with the request")]
    public virtual string State { get; set; }
}

///<summary>
///Generate image from text description
///</summary>
[Api(Description="Generate image from text description")]
public partial class QueueTextToImage
    : IReturn<QueueGenerationResponse>, IQueueGeneration
{
    ///<summary>
    ///The main prompt describing the desired image
    ///</summary>
    [ApiMember(Description="The main prompt describing the desired image")]
    [Validate("NotEmpty")]
    public virtual string PositivePrompt { get; set; }

    ///<summary>
    ///Optional prompt specifying what should not be in the image
    ///</summary>
    [ApiMember(Description="Optional prompt specifying what should not be in the image")]
    public virtual string NegativePrompt { get; set; }

    ///<summary>
    ///Desired width of the generated image
    ///</summary>
    [ApiMember(Description="Desired width of the generated image")]
    public virtual int? Width { get; set; }

    ///<summary>
    ///Desired height of the generated image
    ///</summary>
    [ApiMember(Description="Desired height of the generated image")]
    public virtual int? Height { get; set; }

    ///<summary>
    ///Number of images to generate in a single batch
    ///</summary>
    [ApiMember(Description="Number of images to generate in a single batch")]
    public virtual int? BatchSize { get; set; }

    ///<summary>
    ///The AI model to use for image generation
    ///</summary>
    [ApiMember(Description="The AI model to use for image generation")]
    public virtual string Model { get; set; }

    ///<summary>
    ///Optional seed for reproducible results
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }

    ///<summary>
    ///Optional state to associate with the request
    ///</summary>
    [ApiMember(Description="Optional state to associate with the request")]
    public virtual string State { get; set; }
}

///<summary>
///Convert text to speech
///</summary>
[Api(Description="Convert text to speech")]
public partial class QueueTextToSpeech
    : IReturn<QueueGenerationResponse>, IQueueGeneration
{
    ///<summary>
    ///The text to be converted to speech
    ///</summary>
    [ApiMember(Description="The text to be converted to speech")]
    [Required]
    public virtual string Text { get; set; }

    ///<summary>
    ///Optional seed for reproducible results in speech generation
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results in speech generation")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///The AI model to use for speech generation
    ///</summary>
    [ApiMember(Description="The AI model to use for speech generation")]
    public virtual string Model { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }

    ///<summary>
    ///Optional state to associate with the request
    ///</summary>
    [ApiMember(Description="Optional state to associate with the request")]
    public virtual string State { get; set; }
}

public partial class QueueTrimVideo
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform
{
    ///<summary>
    ///The start time of the trimmed video (format: HH:MM:SS)
    ///</summary>
    [ApiMember(Description="The start time of the trimmed video (format: HH:MM:SS)")]
    [Required]
    public virtual string StartTime { get; set; }

    ///<summary>
    ///The end time of the trimmed video (format: HH:MM:SS)
    ///</summary>
    [ApiMember(Description="The end time of the trimmed video (format: HH:MM:SS)")]
    public virtual string EndTime { get; set; }

    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial class QueueWatermarkImage
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform, IPost
{
    ///<summary>
    ///The image file to be watermarked
    ///</summary>
    [ApiMember(Description="The image file to be watermarked")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///The position of the watermark on the image
    ///</summary>
    [ApiMember(Description="The position of the watermark on the image")]
    public virtual WatermarkPosition Position { get; set; }

    ///<summary>
    ///The opacity of the watermark (0.0 to 1.0)
    ///</summary>
    [ApiMember(Description="The opacity of the watermark (0.0 to 1.0)")]
    public virtual float Opacity { get; set; }

    ///<summary>
    ///Scale of the watermark relative
    ///</summary>
    [ApiMember(Description="Scale of the watermark relative")]
    public virtual float WatermarkScale { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Watermark video
///</summary>
[Api(Description="Watermark video")]
public partial class QueueWatermarkVideo
    : IReturn<QueueMediaTransformResponse>, IQueueMediaTransform
{
    ///<summary>
    ///The video file to be watermarked
    ///</summary>
    [ApiMember(Description="The video file to be watermarked")]
    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///The image file to use as a watermark
    ///</summary>
    [ApiMember(Description="The image file to use as a watermark")]
    [Required]
    public virtual string Watermark { get; set; }

    ///<summary>
    ///Position of the watermark
    ///</summary>
    [ApiMember(Description="Position of the watermark")]
    public virtual WatermarkPosition? Position { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Optional queue or topic to reply to
    ///</summary>
    [ApiMember(Description="Optional queue or topic to reply to")]
    public virtual string ReplyTo { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial class Reload
    : IReturn<EmptyResponse>, IPost
{
}

public enum ResponseFormat
{
    [EnumMember(Value="text")]
    Text,
    [EnumMember(Value="json_object")]
    JsonObject,
}

///<summary>
///Scale an image to a specified size
///</summary>
[Api(Description="Scale an image to a specified size")]
public partial class ScaleImage
    : IReturn<ArtifactGenerationResponse>, IMediaTransform, IPost
{
    ///<summary>
    ///The image file to be scaled
    ///</summary>
    [ApiMember(Description="The image file to be scaled")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///Desired width of the scaled image
    ///</summary>
    [ApiMember(Description="Desired width of the scaled image")]
    public virtual int? Width { get; set; }

    ///<summary>
    ///Desired height of the scaled image
    ///</summary>
    [ApiMember(Description="Desired height of the scaled image")]
    public virtual int? Height { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Scale video
///</summary>
[Api(Description="Scale video")]
public partial class ScaleVideo
    : IReturn<ArtifactGenerationResponse>, IMediaTransform
{
    ///<summary>
    ///The video file to be scaled
    ///</summary>
    [ApiMember(Description="The video file to be scaled")]
    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///Desired width of the scaled video
    ///</summary>
    [ApiMember(Description="Desired width of the scaled video")]
    public virtual int? Width { get; set; }

    ///<summary>
    ///Desired height of the scaled video
    ///</summary>
    [ApiMember(Description="Desired height of the scaled video")]
    public virtual int? Height { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Convert speech to text
///</summary>
[Api(Description="Convert speech to text")]
public partial class SpeechToText
    : IReturn<TextGenerationResponse>, IGeneration
{
    ///<summary>
    ///The audio stream containing the speech to be transcribed
    ///</summary>
    [ApiMember(Description="The audio stream containing the speech to be transcribed")]
    [Required]
    public virtual string Audio { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial class SummaryStats
{
    public virtual string Name { get; set; }
    public virtual int Total { get; set; }
    public virtual int TotalPromptTokens { get; set; }
    public virtual int TotalCompletionTokens { get; set; }
    public virtual double TotalMinutes { get; set; }
    public virtual double TokensPerSecond { get; set; }
}

public enum TaskType
{
    OpenAiChat = 1,
    Comfy = 2,
    OllamaGenerate = 3,
}

///<summary>
///Response object for text generation requests
///</summary>
[Api(Description="Response object for text generation requests")]
public partial class TextGenerationResponse
{
    ///<summary>
    ///List of generated text outputs
    ///</summary>
    [ApiMember(Description="List of generated text outputs")]
    public virtual List<TextOutput> Results { get; set; }

    ///<summary>
    ///Detailed response status information
    ///</summary>
    [ApiMember(Description="Detailed response status information")]
    public virtual ResponseStatus ResponseStatus { get; set; }
}

public partial class TextOutput
{
    ///<summary>
    ///The generated text
    ///</summary>
    [ApiMember(Description="The generated text")]
    public virtual string Text { get; set; }
}

///<summary>
///Generate image from text description
///</summary>
[Api(Description="Generate image from text description")]
public partial class TextToImage
    : IReturn<ArtifactGenerationResponse>, IGeneration
{
    ///<summary>
    ///The main prompt describing the desired image
    ///</summary>
    [ApiMember(Description="The main prompt describing the desired image")]
    [Validate("NotEmpty")]
    public virtual string PositivePrompt { get; set; }

    ///<summary>
    ///Optional prompt specifying what should not be in the image
    ///</summary>
    [ApiMember(Description="Optional prompt specifying what should not be in the image")]
    public virtual string NegativePrompt { get; set; }

    ///<summary>
    ///Desired width of the generated image
    ///</summary>
    [ApiMember(Description="Desired width of the generated image")]
    public virtual int? Width { get; set; }

    ///<summary>
    ///Desired height of the generated image
    ///</summary>
    [ApiMember(Description="Desired height of the generated image")]
    public virtual int? Height { get; set; }

    ///<summary>
    ///Number of images to generate in a single batch
    ///</summary>
    [ApiMember(Description="Number of images to generate in a single batch")]
    public virtual int? BatchSize { get; set; }

    ///<summary>
    ///The AI model to use for image generation
    ///</summary>
    [ApiMember(Description="The AI model to use for image generation")]
    public virtual string Model { get; set; }

    ///<summary>
    ///Optional seed for reproducible results
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///Convert text to speech
///</summary>
[Api(Description="Convert text to speech")]
public partial class TextToSpeech
    : IReturn<ArtifactGenerationResponse>, IGeneration
{
    ///<summary>
    ///The text to be converted to speech
    ///</summary>
    [ApiMember(Description="The text to be converted to speech")]
    [Validate("NotEmpty")]
    public virtual string Input { get; set; }

    ///<summary>
    ///Optional specific model and voice to use for speech generation
    ///</summary>
    [ApiMember(Description="Optional specific model and voice to use for speech generation")]
    public virtual string Model { get; set; }

    ///<summary>
    ///Optional seed for reproducible results in speech generation
    ///</summary>
    [ApiMember(Description="Optional seed for reproducible results in speech generation")]
    public virtual int? Seed { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

///<summary>
///The tool calls generated by the model, such as function calls.
///</summary>
[Api(Description="The tool calls generated by the model, such as function calls.")]
[DataContract]
public partial class ToolCall
{
    ///<summary>
    ///The ID of the tool call.
    ///</summary>
    [DataMember(Name="id")]
    [ApiMember(Description="The ID of the tool call.")]
    public virtual string Id { get; set; }

    ///<summary>
    ///The type of the tool. Currently, only `function` is supported.
    ///</summary>
    [DataMember(Name="type")]
    [ApiMember(Description="The type of the tool. Currently, only `function` is supported.")]
    public virtual string Type { get; set; }

    ///<summary>
    ///The function that the model called.
    ///</summary>
    [DataMember(Name="function")]
    [ApiMember(Description="The function that the model called.")]
    public virtual string Function { get; set; }
}

///<summary>
///Trim a video to a specified duration via start and end times
///</summary>
[Api(Description="Trim a video to a specified duration via start and end times")]
public partial class TrimVideo
    : IReturn<ArtifactGenerationResponse>, IMediaTransform
{
    ///<summary>
    ///The start time of the trimmed video (format: MM:SS)
    ///</summary>
    [ApiMember(Description="The start time of the trimmed video (format: MM:SS)")]
    [Required]
    public virtual string StartTime { get; set; }

    ///<summary>
    ///The end time of the trimmed video (format: MM:SS)
    ///</summary>
    [ApiMember(Description="The end time of the trimmed video (format: MM:SS)")]
    public virtual string EndTime { get; set; }

    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public partial class UpdateAiProvider
    : IReturn<IdResponse>, IPatchDb<AiProvider>
{
    public virtual int Id { get; set; }
    ///<summary>
    ///The Type of this API Provider
    ///</summary>
    [ApiMember(Description="The Type of this API Provider")]
    public virtual string AiTypeId { get; set; }

    ///<summary>
    ///The Base URL for the API Provider
    ///</summary>
    [ApiMember(Description="The Base URL for the API Provider")]
    public virtual string ApiBaseUrl { get; set; }

    ///<summary>
    ///The unique name for this API Provider
    ///</summary>
    [ApiMember(Description="The unique name for this API Provider")]
    public virtual string Name { get; set; }

    ///<summary>
    ///The API Key to use for this Provider
    ///</summary>
    [ApiMember(Description="The API Key to use for this Provider")]
    public virtual string ApiKeyVar { get; set; }

    ///<summary>
    ///The API Key to use for this Provider
    ///</summary>
    [ApiMember(Description="The API Key to use for this Provider")]
    public virtual string ApiKey { get; set; }

    ///<summary>
    ///Send the API Key in the Header instead of Authorization Bearer
    ///</summary>
    [ApiMember(Description="Send the API Key in the Header instead of Authorization Bearer")]
    public virtual string ApiKeyHeader { get; set; }

    ///<summary>
    ///The URL to check if the API Provider is still online
    ///</summary>
    [ApiMember(Description="The URL to check if the API Provider is still online")]
    public virtual string HeartbeatUrl { get; set; }

    ///<summary>
    ///Override API Paths for different AI Requests
    ///</summary>
    [ApiMember(Description="Override API Paths for different AI Requests")]
    public virtual Dictionary<TaskType, string> TaskPaths { get; set; }

    ///<summary>
    ///How many requests should be made concurrently
    ///</summary>
    [ApiMember(Description="How many requests should be made concurrently")]
    public virtual int? Concurrency { get; set; }

    ///<summary>
    ///What priority to give this Provider to use for processing models
    ///</summary>
    [ApiMember(Description="What priority to give this Provider to use for processing models")]
    public virtual int? Priority { get; set; }

    ///<summary>
    ///Whether the Provider is enabled
    ///</summary>
    [ApiMember(Description="Whether the Provider is enabled")]
    public virtual bool? Enabled { get; set; }

    ///<summary>
    ///The models this API Provider should process
    ///</summary>
    [ApiMember(Description="The models this API Provider should process")]
    public virtual List<AiProviderModel> Models { get; set; }

    ///<summary>
    ///The selected models this API Provider should process
    ///</summary>
    [ApiMember(Description="The selected models this API Provider should process")]
    public virtual List<string> SelectedModels { get; set; }
}

///<summary>
///Update a Generation API Provider
///</summary>
[Api(Description="Update a Generation API Provider")]
public partial class UpdateMediaProvider
    : IReturn<IdResponse>, IPatchDb<MediaProvider>
{
    public virtual int Id { get; set; }
    ///<summary>
    ///The API Key to use for this Provider
    ///</summary>
    [ApiMember(Description="The API Key to use for this Provider")]
    public virtual string ApiKey { get; set; }

    ///<summary>
    ///Send the API Key in the Header instead of Authorization Bearer
    ///</summary>
    [ApiMember(Description="Send the API Key in the Header instead of Authorization Bearer")]
    public virtual string ApiKeyHeader { get; set; }

    ///<summary>
    ///Override Base URL for the Generation Provider
    ///</summary>
    [ApiMember(Description="Override Base URL for the Generation Provider")]
    public virtual string ApiBaseUrl { get; set; }

    ///<summary>
    ///Url to check if the API is online
    ///</summary>
    [ApiMember(Description="Url to check if the API is online")]
    public virtual string HeartbeatUrl { get; set; }

    ///<summary>
    ///How many requests should be made concurrently
    ///</summary>
    [ApiMember(Description="How many requests should be made concurrently")]
    public virtual int? Concurrency { get; set; }

    ///<summary>
    ///What priority to give this Provider to use for processing models
    ///</summary>
    [ApiMember(Description="What priority to give this Provider to use for processing models")]
    public virtual int? Priority { get; set; }

    ///<summary>
    ///Whether the Provider is enabled
    ///</summary>
    [ApiMember(Description="Whether the Provider is enabled")]
    public virtual bool? Enabled { get; set; }

    ///<summary>
    ///The models this API Provider should process
    ///</summary>
    [ApiMember(Description="The models this API Provider should process")]
    public virtual List<string> Models { get; set; }
}

public partial class WaitForOpenAiChat
    : IReturn<GetOpenAiChatResponse>, IGet
{
    public virtual int? Id { get; set; }
    public virtual string RefId { get; set; }
}

///<summary>
///Add a watermark to an image
///</summary>
[Api(Description="Add a watermark to an image")]
public partial class WatermarkImage
    : IReturn<ArtifactGenerationResponse>, IMediaTransform, IPost
{
    ///<summary>
    ///The image file to be watermarked
    ///</summary>
    [ApiMember(Description="The image file to be watermarked")]
    [Required]
    public virtual string Image { get; set; }

    ///<summary>
    ///The position of the watermark on the image
    ///</summary>
    [ApiMember(Description="The position of the watermark on the image")]
    public virtual WatermarkPosition Position { get; set; }

    ///<summary>
    ///Scale of the watermark relative
    ///</summary>
    [ApiMember(Description="Scale of the watermark relative")]
    public virtual float WatermarkScale { get; set; }

    ///<summary>
    ///The opacity of the watermark (0.0 to 1.0)
    ///</summary>
    [ApiMember(Description="The opacity of the watermark (0.0 to 1.0)")]
    public virtual float Opacity { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public enum WatermarkPosition
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
    Center,
}

///<summary>
///Watermark video
///</summary>
[Api(Description="Watermark video")]
public partial class WatermarkVideo
    : IReturn<ArtifactGenerationResponse>, IMediaTransform
{
    ///<summary>
    ///The video file to be watermarked
    ///</summary>
    [ApiMember(Description="The video file to be watermarked")]
    [Required]
    public virtual string Video { get; set; }

    ///<summary>
    ///The image file to use as a watermark
    ///</summary>
    [ApiMember(Description="The image file to use as a watermark")]
    [Required]
    public virtual string Watermark { get; set; }

    ///<summary>
    ///Position of the watermark
    ///</summary>
    [ApiMember(Description="Position of the watermark")]
    public virtual WatermarkPosition? Position { get; set; }

    ///<summary>
    ///Optional client-provided identifier for the request
    ///</summary>
    [ApiMember(Description="Optional client-provided identifier for the request")]
    public virtual string RefId { get; set; }

    ///<summary>
    ///Tag to identify the request
    ///</summary>
    [ApiMember(Description="Tag to identify the request")]
    public virtual string Tag { get; set; }
}

public enum AiServiceProvider
{
    Replicate,
    Comfy,
    OpenAi,
}

public enum ComfyMaskSource
{
    red,
    blue,
    green,
    alpha,
}

public enum ComfySampler
{
    euler,
    euler_cfg_pp,
    euler_ancestral,
    euler_ancestral_cfg_pp,
    huen,
    huenpp2,
    dpm_2,
    dpm_2_ancestral,
    lms,
    dpm_fast,
    dpm_adaptive,
    dpmpp_2s_ancestral,
    dpmpp_sde,
    dpmpp_sde_gpu,
    dpmpp_2m,
    dpmpp_2m_sde,
    dpmpp_2m_sde_gpu,
    dpmpp_3m_sde,
    dpmpp_3m_sde_gpu,
    ddpm,
    lcm,
    ddim,
    uni_pc,
    uni_pc_bh2,
}

public partial class MediaModel
{
    public virtual string Id { get; set; }
    public virtual string Name { get; set; }
    public virtual ModelType? Type { get; set; }
    public virtual Dictionary<string, string> ApiModels { get; set; } = new();
    public virtual Dictionary<string, List<String>> SupportedTasks { get; set; }
    public virtual List<string> Dependencies { get; set; }
    public virtual string Installer { get; set; }
    public virtual string Path { get; set; }
    public virtual string Workflow { get; set; }
    public virtual Dictionary<string, Object> WorkflowVars { get; set; } = new();
    public virtual string DownloadToken { get; set; }
    public virtual string DownloadUrl { get; set; }
    public virtual string Url { get; set; }
}

public partial class MediaProvider
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string ApiKeyVar { get; set; }
    public virtual string ApiUrlVar { get; set; }
    public virtual string ApiKey { get; set; }
    public virtual string ApiKeyHeader { get; set; }
    public virtual string ApiBaseUrl { get; set; }
    public virtual string HeartbeatUrl { get; set; }
    public virtual int Concurrency { get; set; }
    public virtual int Priority { get; set; }
    public virtual bool Enabled { get; set; }
    public virtual DateTime? OfflineDate { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual string MediaTypeId { get; set; }
    [Ignore]
    public virtual MediaType MediaType { get; set; }

    public virtual List<string> Models { get; set; } = [];
}

public partial class MediaType
{
    public virtual string Id { get; set; }
    public virtual string ApiBaseUrl { get; set; }
    public virtual string ApiKeyHeader { get; set; }
    public virtual string Website { get; set; }
    public virtual string Icon { get; set; }
    public virtual Dictionary<string, string> ApiModels { get; set; } = new();
    public virtual AiServiceProvider Provider { get; set; }
}

public enum ModelType
{
    TextToImage,
    TextEncoder,
    ImageUpscale,
    TextToSpeech,
    TextToAudio,
    SpeechToText,
    ImageToText,
    ImageToImage,
    ImageWithMask,
    Lora,
    VAE,
}

public partial class QueryMediaTypes
    : QueryDb<MediaType>, IReturn<QueryResponse<MediaType>>
{
}

public partial class TextToSpeechVoice
{
    public virtual string Id { get; set; }
    public virtual string Model { get; set; }
}