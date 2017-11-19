# SendMail
The solution is created on Visual Studion 2015

There are two projects
1. API
2. Web

Create a site on your local IIS.
Publish the API by right clicking on the project and selecting "Publish". Choose WebDeploy as the publish method and choose the site that you
created.
Once the publish is done, copy the content of the published output from the IIS directory of the site and copy to the IIS site on the server
where you would run the API service.

Similar to the above, publish the Web project and copy the ouput to the IIS site of the server where you would run the front end.
Copy the Url of the API site in the Web.Config file of the Web (front end) site in the following entry:
   <add key="APIUrl" value="http://localhost:8085/" />

The "value" above should point to your API url.

Now run your front end site
