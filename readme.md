# RazorEngine Complete

RazorEngine is a useful tool for using Razor outside the normal Asp.Net MVC flow - for example, when generating HTML emails.

But, RazorEngine does not include some important components one would expect from using Razor in its usual context - in particular, the critically important `@Html.Raw()`. It also doesn't support `@Html.Partial`, and to what degree it supports an equivalent (`@Include`), you've got some work ahead of you.

While you could use some of their alternatives, you then lose the ability to do the obvious - test your Html emails using stub endpoints on regular ol' MVC - because now you've swapped `@Html.Partial` for `@Include`, which is not supported in the MVC version of Razor... .

This library gets RazorEngine working out of the box, using the calls you're used to. `@Html.Raw()` and `@Html.Partial()` both work, and Partial is even smart enough to navigate Areas and Shared folders, like `~/Views/Shared`.

## Usage

Include all the Brass9.* libraries in your Solution, and Reference them. You don't need the RazorEngineMvc Project - that's just sample code to demonstrate how the library works.

Then use:

	using Brass.Web.RazorEngining;
	
	. . .

	var razor = RazorHelper.O;
	var html = razor.RenderFromMvc(@"Views\RazorEngine\TestEmail.cshtml", vm);
	
Here we're getting the razor singleton (just for good form), then rendering our Razor file out to a string. See the example code in the RazorEngineMvc sample Project for more.

### Setup / Dependency Injection

This library uses an important setup step, that initializes the RazorHelper singleton. In Global.asax.cs there's a call to:

	string appRoot = Server.MapPath("~/");
	App.Init(appRoot);
	App.O.InitModules();

Which in turn calls

	Brass9.Web.AutoInit.AppAutoInit.Init(...
	
If you're not sure what that means, just add the above code to your Global.asax.cs, copy the App class from RazorEngineMvc into your Project, and you're good.

If you want to get fancy, this is just a Dependency Injection operation - if you have your own IoC Container go for it. In short, RazorHelper needs to know the path to the root Project folder is in order to effectively do its work. It's common for code to need that, so it's stored in a global object, `App`. That object is injected into any class decorated with marker Interface `IAppAutoInit`. `AppAutoInit` finds `RazorHelper`; it's initialized and consumes the `AppRoot` value.

## Sample Code

Included is an example email call and all the code for that call to work, including a small library capable of parsing images in your Razor code to Email Attachments, and deal with `async`. The one thing that is not included is your SMTP info - you'll need to provide your own  credentials to test sending. Look in `HomeController.SendTestEmail()`. 