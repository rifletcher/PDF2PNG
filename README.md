# PDF2PNG

Docker

docker build -t rifletcher/pdf-converter:v1.0.0 .
docker login
docker push rifletcher/pdf-converter:v1.0.0
docker run -d -p5050 rifletcher/pdf-converter:v1.0.0
docker ps

Kubernetes
gcloud container clusters create gfs-ecm --num-nodes=3 --zone=europe-west1-b
kubectl create -f k8s-deploy
kubectl apply -f <filename.yaml>
kubectl set image deployment/nginx-deployment nginx=nginx:1.9.1

MiniKube
minikube start --vm-driver hyperv --hyperv-virtual-switch "My Virtual Switch" --v=0
minikube addons enable ingress
minikube dashboard
minikube delete --C:\Users\filename\.minikube
