#!/bin/bash

version=$1

# First we build the images
sudo docker build . -t  feldrise/isati-integration-api 

# The we add tag for version
sudo docker image tag feldrise/isati-integration-api:latest feldrise/isati-integration-api:$version

# Finally we push to Docker hub
sudo docker push feldrise/isati-integration-api:latest
sudo docker push feldrise/isati-integration-api:$version