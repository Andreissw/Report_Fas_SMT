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
    
    public partial class FASEntities : DbContext
    {
        public FASEntities()
            : base("name=FASEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<FAS_Objective> FAS_Objective { get; set; }
        public virtual DbSet<FAS_Users> FAS_Users { get; set; }
        public virtual DbSet<Ct_StepScan> Ct_StepScan { get; set; }
        public virtual DbSet<Ct_TestResult> Ct_TestResult { get; set; }
        public virtual DbSet<FAS_DefectCode> FAS_DefectCode { get; set; }
        public virtual DbSet<FAS_Disassembly> FAS_Disassembly { get; set; }
        public virtual DbSet<FAS_ErrorCode> FAS_ErrorCode { get; set; }
        public virtual DbSet<FAS_GS_LOTs> FAS_GS_LOTs { get; set; }
        public virtual DbSet<FAS_Lines> FAS_Lines { get; set; }
        public virtual DbSet<FAS_Models> FAS_Models { get; set; }
        public virtual DbSet<FAS_PackingGS> FAS_PackingGS { get; set; }
        public virtual DbSet<FAS_RepairCode> FAS_RepairCode { get; set; }
        public virtual DbSet<FAS_SerialNumbers> FAS_SerialNumbers { get; set; }
        public virtual DbSet<FAS_Start> FAS_Start { get; set; }
        public virtual DbSet<FAS_Upload> FAS_Upload { get; set; }
        public virtual DbSet<Ct_OperLog> Ct_OperLog { get; set; }
        public virtual DbSet<Contract_LOT> Contract_LOT { get; set; }
        public virtual DbSet<FAS_Liter> FAS_Liter { get; set; }
        public virtual DbSet<M_Repair_Table> M_Repair_Table { get; set; }
        public virtual DbSet<KGP_Control> KGP_Control { get; set; }
        public virtual DbSet<KGP_Control_Sputnik> KGP_Control_Sputnik { get; set; }
    }
}