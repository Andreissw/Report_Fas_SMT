﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Report_Fas_SMT
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FASEntities1 : DbContext
    {
        public FASEntities1()
            : base("name=FASEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Contract_LOT> Contract_LOT { get; set; }
        public virtual DbSet<Ct_PackingTable> Ct_PackingTable { get; set; }
        public virtual DbSet<Ct_StepResult> Ct_StepResult { get; set; }
        public virtual DbSet<Ct_StepScan> Ct_StepScan { get; set; }
        public virtual DbSet<FAS_DefectCode> FAS_DefectCode { get; set; }
        public virtual DbSet<FAS_Disassembly> FAS_Disassembly { get; set; }
        public virtual DbSet<FAS_ErrorCode> FAS_ErrorCode { get; set; }
        public virtual DbSet<FAS_ErrorCodeGroup> FAS_ErrorCodeGroup { get; set; }
        public virtual DbSet<FAS_GS_LOTs> FAS_GS_LOTs { get; set; }
        public virtual DbSet<FAS_LabelScenario> FAS_LabelScenario { get; set; }
        public virtual DbSet<FAS_Lines> FAS_Lines { get; set; }
        public virtual DbSet<FAS_Liter> FAS_Liter { get; set; }
        public virtual DbSet<FAS_Models> FAS_Models { get; set; }
        public virtual DbSet<FAS_OperationLog> FAS_OperationLog { get; set; }
        public virtual DbSet<FAS_PackingCounter> FAS_PackingCounter { get; set; }
        public virtual DbSet<FAS_PackingGS> FAS_PackingGS { get; set; }
        public virtual DbSet<FAS_RepairCode> FAS_RepairCode { get; set; }
        public virtual DbSet<FAS_SerialNumbers> FAS_SerialNumbers { get; set; }
        public virtual DbSet<FAS_ShiftPlan> FAS_ShiftPlan { get; set; }
        public virtual DbSet<FAS_Start> FAS_Start { get; set; }
        public virtual DbSet<FAS_Upload> FAS_Upload { get; set; }
        public virtual DbSet<FAS_Users> FAS_Users { get; set; }
        public virtual DbSet<KGP_Control> KGP_Control { get; set; }
        public virtual DbSet<KGP_Control_Sputnik> KGP_Control_Sputnik { get; set; }
        public virtual DbSet<Ct_FASSN_reg> Ct_FASSN_reg { get; set; }
        public virtual DbSet<Ct_TestResult> Ct_TestResult { get; set; }
        public virtual DbSet<M_Repair_Table> M_Repair_Table { get; set; }
        public virtual DbSet<Ct_OperLog> Ct_OperLog { get; set; }
        public virtual DbSet<FAS_Objective> FAS_Objective { get; set; }
        public virtual DbSet<EP_Protocols> EP_Protocols { get; set; }
        public virtual DbSet<EP_PGName> EP_PGName { get; set; }
    }
}
