﻿using org.activiti.cloud.services.api.model;
using org.activiti.cloud.services.rest.api.resources;
using org.activiti.cloud.services.rest.controllers;
using org.springframework.hateoas;
using org.springframework.hateoas.mvc;
using System;

namespace org.activiti.cloud.services.rest.assemblers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessDefinitionMetaResourceAssembler : ResourceAssemblerSupport<ProcessDefinitionMeta, ProcessDefinitionMetaResource>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcessDefinitionMetaResourceAssembler() : base(typeof(ProcessDefinitionMetaControllerImpl), typeof(ProcessDefinitionMetaResource))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processDefinitionMeta"></param>
        /// <returns></returns>
        public override ProcessDefinitionMetaResource toResource(ProcessDefinitionMeta processDefinitionMeta)
        {
            //throw new NotImplementedException();
            //Link metadata = linkTo(methodOn(typeof(ProcessDefinitionMetaControllerImpl)).getProcessDefinitionMetadata(processDefinitionMeta.Id)).withRel("meta");
            //Link selfRel = linkTo(methodOn(typeof(ProcessDefinitionControllerImpl)).getProcessDefinition(processDefinitionMeta.Id)).withSelfRel();
            //Link startProcessLink = linkTo(methodOn(typeof(ProcessInstanceControllerImpl)).startProcess(null)).withRel("startProcess");
            //Link homeLink = linkTo(typeof(HomeControllerImpl)).withRel("home");

            return new ProcessDefinitionMetaResource(processDefinitionMeta, null);
        }
    }

}