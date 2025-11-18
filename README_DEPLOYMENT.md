# Deploying ASP.NET Application to Google Cloud Platform's Cloud Run

## 1. Download Google CLI for Windows

Install the [Google Cloud SDK](https://cloud.google.com/sdk/docs/install) to get started.

## 2. Log In to Google Cloud

Login and authenticate your account:

```powershell
gcloud auth login
```

## 3. Configure Docker in PowerShell

Enable Docker to push images to Google Container Registry:

```powershell
gcloud auth configure-docker
```

## 4. Build Dockerfile and Add to Project Root

-   Notes:
    -   `AgenticGreenthumbApi` is my project name, use your own project name for you project.
    -   `mcr.microsoft.com/dotnet/sdk:9.0` - I am using .NET 9.0, if your application is using a different version, adjust it to that

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set environment and port for Cloud Run
ENV ASPNETCORE_ENVIRONMENT=Production
ENV PORT=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AgenticGreenthumbApi.dll"]
```

## 5. Build the Docker Image

Run this from the root project folder containing your Dockerfile. Make sure Docker Desktop is Running:

```powershell
docker build -t gcr.io/{google-cloud-project-name}/{image-name} .
```

```powershell
docker build -t gcr.io/nguyen-iot-prototype/agentic-greenthumb-api .
```

---

## 5A. Optional: Retag the Image

If needed, retag your image to match Google’s naming convention:

```powershell
docker tag {image-name} gcr.io/{my-project}/{image-name}
```

## 6. Push the Image to Google Container Registry

```powershell
docker push gcr.io/nguyen-iot-prototype/agentic-greenthumb-api
```

## 7. Deploy to Cloud Run

Use PowerShell or terminal:

```powershell
gcloud run deploy agentic-greenthumb-api \
  --image gcr.io/nguyen-iot-prototype/agentic-greenthumb-api \
  --platform managed \
  --region us-central1 \
  --allow-unauthenticated \
  --port 8080 \
  --set-env-vars ASPNETCORE_ENVIRONMENT=Production
```

---

# Deploying ANGULAR FRONTEND via Google Cloud Platform's Cloud Run

-   **Note:**
-   Before starting these steps, make sure you have the production build (`dist` folder) of your web application and a valid `app.yaml` configuration file.

## 1. Clone the App Engine Frontend Folder

Copy the folder and its contents from the GitHub repository to your local machine:

## 2. Move Your Angular `dist` Folder

Place your Angular project’s `dist` folder into the `front_end_app_folder` folder you just copied.

## 3. Configure `app.yaml`

Open or create the `app.yaml` file inside `app_engine_frontend`. Update it to reflect the correct path to your production files inside the `dist` subfolder.

Example `app.yaml`:

```yaml
runtime: nodejs22

instance_class: F2

handlers:
    - url: /
      static_files: dist/PATH/TO/FILES/index.html
      upload: dist/PATH/TO/FILES/index.html

    - url: /
      static_dir: dist/PATH/TO/FILES
```

-   Note: If your app’s base URL is not `/`, update the `url` field accordingly.

## 4. Upload to Google Cloud Shell

Transfer the modified `front_end_app_folder` folder to your Cloud Shell environment.

-   You do **not** need to include `package.json` or `angular.json` unless you plan to build in the cloud. These files trigger build + deploy, not just deploy.

## 5. Navigate to the Folder in Cloud Shell

```cloud shell
cd front_end_app_folder
```

## 6. Deploy to App Engine

Run the deployment command:

```cloud shell
gcloud app deploy
```

## 7. Choose Hosting Region

When prompted, select the same region used for your other microservices (e.g., `us-central1`).

## 8. Confirm Deployment

Accept the prompt to confirm deployment.

## 9. Access Your App

Once deployed, you’ll receive a public URL like:

```
https://ringed-bond-99999-i3.uc.r.appspot.com/
```

Open it in your browser — hopefully, it works!

---
