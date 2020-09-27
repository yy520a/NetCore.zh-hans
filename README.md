# NetCore.zh-hans
1.使用之前先申请百度翻译API，申请接口地址 https://api.fanyi.baidu.com/ ，申请通用高级版即可，每月可免费翻译200万字符，申请到appId与secretKey后，填入对应的TextBox;

2. 点击 批量导入Xml文件|开始翻译按钮，导入AspNetCore.App.Ref 英文Xml文件， 默认路径是： C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\3.1.8\ref\netcoreapp3.1 ，全选该文件夹里所有的.xml文件 然后点击打开就开始批量翻译了，注意观察进度条，全部翻译完成大概在2-3小时，共计130万左右字符翻译量，请放心，程序不会修改原版的Xml文件，全部翻译完成后，会自动输出保存翻译结果到本项目目录内：Debug\netcoreapp3.1\zh-hans；

3.复制zh-hans目录到C:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\3.1.8\ref\netcoreapp3.1 ，然后重启VS ，AspNetCore.App.Ref相关的方法“摘要”智能提示时会变成 原文+百度译文：  Visual Studio 2019 测试通过;


