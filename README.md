# Qiniu (Cloud) C# SDK

#### 介绍
轻量级、高性能的 Qiniu (Cloud) C# SDK，不依赖 `Newtonsoft` 库。

#### 注册服务  
```csharp
services.AddQiniuService(config =>
{
    config.Mac = new("<AccessKeyId>", "<AccessKeySecret>");
    config.Zone = Zone.ZONE_CN_South; //存储区域
    config.UseHttps = true;
    config.UseCdnDomains = true;
});
```

#### 使用说明
- 上传前处理
```csharp
//获取注册的服务
IQiniuService service = provider.GetRequiredService<IQiniuService>(); //生产环境通过DI获取 IQiniuService
```

- 文件上传
```csharp
//上传策略 https://developer.qiniu.com/kodo/manual/put-policy
PutPolicy putPolicy = new() { 
    Scope = "<Bucket>:<Key>", //如果仅设置 Bucket 表示同名Key不允许覆盖
    SaveKey = "qiniu.png"
};

//通过字节流上传
var buff = File.ReadAllBytes(@"C:\qiniu.png");
var result = await service.UploadDataAsync(buff, putPolicy);

//通过文件流上传
using var fs = File.OpenRead(@"C:\qiniu.png");
var result = await service.UploadStreamAsync(fs, putPolicy);

```

- 单文件删除
```csharp
var result = await service.DeleteAsync(bucket, "qiniu.png");
```

- 批量删除
```csharp
var result = await service.BatchDeleteAsync(bucket, ["x.png", "x2.png", "x3.png"]);
```

- 修改文件过期删除时间
```csharp
var result = await service.DeleteAfterDaysAsync(bucket, "qiniu.png", 3);
```

- 批量修改文件过期删除时间
```csharp
var result = await service.BatchDeleteAfterDaysAsync(bucket, ["x.png", "x2.png", "x3.png"], 3);
```

可通过 `SendAsync` 来扩展更多操作。
