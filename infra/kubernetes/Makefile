SHELL:=/bin/bash
KUBECTL_CONTEXT:=$(ENV)
KUBECTL_BIN:=kubectl
PORT_FORWARD?=8080:8080
KUBECTL_NAMESPACE:=testapp

ifeq ($(KUBECTL_CONTEXT),local)
KUBECTL_CONTEXT:=docker-desktop
endif

kubectl/configure:
ifndef ENV
	@echo "	make ENV=local $(MAKECMDGOALS)"
	@exit 1
endif
ifneq ($(shell $(KUBECTL_BIN) config current-context),$(KUBECTL_CONTEXT))
	@echo "	kubectl config use-context $(KUBECTL_CONTEXT)"
	@exit 1
endif
ifeq (,$(wildcard $(CURDIR)/config/$(ENV)/kustomization.yaml))
	@echo "Invalid environment $(ENV)"
	@exit 1
endif
	@$(foreach f, $(wildcard $(CURDIR)/config/$(ENV)/*/*/config.env.dist), test -f $(dir $f)/config.env || cp $(f) $(dir $f)/config.env;)
	@$(foreach f, $(wildcard $(CURDIR)/config/$(ENV)/*/*/secrets.env.dist), test -f $(dir $f)/secrets.env || cp $(f) $(dir $f)/secrets.env;)

kubectl/apply: kubectl/configure
	$(KUBECTL_BIN) apply -k $(CURDIR)/config/$(ENV) $(KUBECTL_ARGS)

kubectl/delete: kubectl/configure
	$(KUBECTL_BIN) delete -k $(CURDIR)/config/$(ENV) $(KUBECTL_ARGS)

kubectl/kustomize: kubectl/configure
	$(KUBECTL_BIN) kustomize $(CURDIR)/config/$(ENV) $(KUBECTL_ARGS)

kubectl/diff: kubectl/configure
	$(KUBECTL_BIN) diff -k $(CURDIR)/config/$(ENV) $(KUBECTL_ARGS)

kubectl/dry-run: kubectl/configure
	$(KUBECTL_BIN) apply -k $(CURDIR)/config/$(ENV) --dry-run $(KUBECTL_ARGS)

kubectl/pods/testapp: kubectl/configure
	$(KUBECTL_BIN) -n $(KUBECTL_NAMESPACE) get pods $(KUBECTL_ARGS)

kubectl/services/testapp: kubectl/configure
	$(KUBECTL_BIN) -n $(KUBECTL_NAMESPACE) get services $(KUBECTL_ARGS)

kubectl/logs/testapp/%: kubectl/configure
	$(KUBECTL_BIN) -n $(KUBECTL_NAMESPACE) logs -f -l app=testapp-$(notdir $@) $(KUBECTL_ARGS)

kubectl/reload/testapp/%: kubectl/configure
	$(KUBECTL_BIN) -n $(KUBECTL_NAMESPACE) delete pod -l app=testapp-$(notdir $@) $(KUBECTL_ARGS)

kubectl/port-forward/testapp/%: kubectl/configure
	$(KUBECTL_BIN) -n $(KUBECTL_NAMESPACE) port-forward service/testapp-$(notdir $@) $(KUBECTL_ARGS) $(PORT_FORWARD)

kubectl/exec/testapp/%: kubectl/configure
	$(KUBECTL_BIN) -n $(KUBECTL_NAMESPACE) exec -it service/testapp-$(notdir $@) $(KUBECTL_ARGS)  -it ash

