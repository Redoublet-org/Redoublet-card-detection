# Redoublet-card-detection
Card detection backend. Asp.NET Core API with wrapper for darknet for image classification. 

## Card detection API
### Steps for running
1. Navigate to `/card-detection-api`
1. In the command line, run `docker-compose build` to build the latest API docker image
1. Once building is completed, run `docker-compose up` to start the API. By default, it will run on [http://localhost:5050](http://localhost:5050/swagger)

### API Endpoint
`/detect-card` takes an image as a base64 encoded JSON string, and outputs a DetectionResult. See the [Swagger](http://localhost:5050/swagger) page for full definition.  