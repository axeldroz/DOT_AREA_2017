DOT_AREA_2017
===================

This is an Epitech project written in c# with .NET Framework.
The main goal of the projet is to create **Webservices** dedicated to **have reactions when a user make an action on a existing platform** (Like *facebook*, *google*, *linkedin* etc ...).
For example if the action is "like a facebook page" the reaction could be "post on my wall i liked a page".

----------


How it Works
-------------

First of all you need to edit the **AREA/Controller/LoginController.cs** file in order to set the ID of your **facebook application.**

```
Facebook.FacebookClient fb = new Facebook.FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "<Insert Your application ID>",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email,publish_actions,user_posts,publish_pages,manage_pages,user_likes"
            });
///////////////////////////////////////////
///////////////////////////////////////////
dynamic result = await fb.PostTaskAsync("oauth/access_token",
            new
            {
                client_id = "<Insert your application ID>",
                client_secret = "<Insert your password if the application hasn't been approved by facebook>",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

```

> **Note:**

> - You won't be able to compile the project if you miss this step.

Now you can build the project and enjoy !
-------------
