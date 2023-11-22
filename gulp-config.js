module.exports = function() {
    var instanceRoot = "C:\\inetpub\\wwwroot\\onewebsc.dev.local";
    var config = {
        websiteRoot: instanceRoot + "\\",
        websiteUrl: "https://onewebsc.dev.local",
        sitecoreLibraries: instanceRoot + "\\bin",
        licensePath: instanceRoot + "\\App_Data\\license.xml",
        packageXmlBasePath: ".\\src\\Project\\Habitat\\code\\App_Data\\packages\\habitat.xml",
        packagePath: instanceRoot + "\\App_Data\\packages",
        solutionName: "OneWeb",
        buildConfiguration: "debug",
        buildToolsVersion: "16.0",
        buildMaxCpuCount: 0,
        buildVerbosity: "minimal",
        buildPlatform: "Any CPU",
        publishPlatform: "AnyCpu",
        powershellUserName: "sitecore\\admin",
        powershellPassword: "b",
        runCleanBuilds: false,
        businessUnitList: ["OneWeb"]
    };
    return config;
}