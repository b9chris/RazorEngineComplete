#RazorEngine Complete

RazorEngine is a useful tool for using Razor outside the normal Asp.Net MVC flow - for example, when generating HTML emails.

But, RazorEngine does not include some important components one would expect from using Razor in its usual context - in particular, the critically important `@Html.Raw()`. It also doesn't support `@Html.Partial`, and to what degree it supports an equivalent (`@Include`), you've got some work ahead of you.

While you could use some of their alternatives, you then lose the ability to do the obvious - test your Html emails using stub endpoints on regular ol' MVC - because now you've swapped @Html.Partial for @Include, which is not supported in the MVC version of Razor... .

This library gets RazorEngine working out of the box, using the calls you're used to. @Html.Raw() and @Html.Partial() both work, and Partial is even smart enough to navigate Areas, if you happen to structure your Razor views deep in Areas and then call Partials from ~/Views/Shared or other Shared folders.

##Usage

	var razor = RazorHelper.O;
	var html = razor.RenderFromMvc(@"Views\RazorEngine\TestEmail.cshtml", vm);
	
Here we're getting the razor singleton (just for good form), then rendering our Razor file out to a string. In this case that Razor file does include a Partial, just to prove it's possible. See the example code below for more.

###Setup

This library uses an important setup step, that initializes the RazorHelper singleton. In Global.asax.cs there's a call to:

	string appRoot = Server.MapPath("~/");
	App.Init(appRoot);
	App.O.InitModules();

Which in turn calls

	Brass9.Web.AutoInit.AppAutoInit.Init(...
	
This is just a Dependency Injection operation - if you have your IoC Container go for it. If not it's all included in this library. In short, RazorHelper needs to know where the root Project folder is in order to effectively do its work. It's common for code to need that, so it's stored in a global object, App. That object is injected into any class decorated with marker Interface `IAppAutoInit`, and from there RazorHelper is initialized and consumes the AppRoot value.

##Example Code

Included is an example email call and all the code for that call to work. The one thing that is not included is a mail server - you'll need to provide your own and its credentials. Look in `HomeController.SendTestEmail()`. 