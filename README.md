# TestApp.

__Table of Contents__
### [Infrastructure](infra)
*  [Kubernetes](infra/kubernetes)

### [Modules]()
* [App](App)
* [Api](Api)

## Read
- [Docker](https://docs.docker.com/get-started/)
- [Kubernetes](https://kubernetes.io/docs/tutorials/kubernetes-basics/)

## Prerequisite
* [Docker](https://docs.docker.com/docker-for-mac/install)
* [Kubernetes](https://docs.docker.com/docker-for-mac/#kubernetes)

## Setup

```bash
git clone git@github.com:kinncj/sample-dotnet-polyrepo.git
```

### Compile the code

```bash
cd ~/path-to/sample-dotnet-polyrepo/
```

```bash
make ENV=local docker/build
```

```bash
make ENV=local  DOCKER_PLATFORM="--platform=linux/arm64" docker/build
```

### Run the kubernetes manifests

```bash
kubectl config get-contexts
# CURRENT NAME
# * docker-desktop
#   remote-cluster
```

```bash
kubectl config use-context docker-desktop
```

```bash
ENV=local make kubectl/kustomize

ENV=local make kubectl/dry-run
```

```bash
ENV=local make docker/build kubectl/apply
```
```bash
kubectl get pods -A

    make ENV=local kubectl/logs/testapp/api
    make ENV=local kubectl/logs/testapp/app
```

```bash
kubectl get pods -A

kubectl -n <NAMESPACE> port-forward <POD NAME> <LOCAL PORT>:<POD PORT>
```

```bash
make ENV=local kubectl/port-forward/testapp/app

open http://localhost:8080/swagger/index.html
```

```bash
# Build and reload pods
make ENV=local docker/build kubectl/reload/testapp/{app,api}
```