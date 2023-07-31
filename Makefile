SHELL:=/bin/bash
DOCKER_IMAGE_TAG:=latest
DOCKER_REPOSITORY_BASE:=my.docker.repository.com/testapp

docker/build:
	@$(foreach f, $(wildcard $(CURDIR)/*/Dockerfile), make docker/build/$(lastword $(subst /, ,$(dir $f)));)

docker/build/%: $(HOOK_PRE_DOCKER_BUILD)
ifndef DOCKER_IMAGE_TAG
	@echo "DOCKER_IMAGE_TAG not defined, try :"
	@echo "	make DOCKER_IMAGE_TAG=latest $(MAKECMDGOALS)"
	@echo
	@exit 1
endif
	docker build . -f $(CURDIR)/$(shell echo $(notdir $@) | tr A-Z a-z)/Dockerfile -t $(DOCKER_REPOSITORY_BASE)/$(shell echo $(notdir $@) | tr A-Z a-z):$(DOCKER_IMAGE_TAG) --build-arg DOCKER_IMAGE_TAG=$(DOCKER_IMAGE_TAG) $(DOCKER_BUILD_CMD_ARGS)

kubectl/%:
ifndef ENV
	@echo "ENV not defined, try :"
	@echo "	make ENV=local $(MAKECMDGOALS)"
	@echo
	@exit 1
endif
	@make ENV=$(ENV) -C $(CURDIR)/infra/kubernetes $@
