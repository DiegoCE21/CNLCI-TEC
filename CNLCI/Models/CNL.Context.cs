﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CNLCI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DB : DbContext
    {
        public DB()
            : base("name=DB")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Alumno> Alumno { get; set; }
        public virtual DbSet<bitacora> bitacora { get; set; }
        public virtual DbSet<Carrera> Carrera { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<Justificante> Justificante { get; set; }
        public virtual DbSet<Medico> Medico { get; set; }
        public virtual DbSet<Pase_de_salida> Pase_de_salida { get; set; }
        public virtual DbSet<Tutor> Tutor { get; set; }
        public virtual DbSet<Ingreso> Ingreso { get; set; }
    
        public virtual int Activar(string identificador)
        {
            var identificadorParameter = identificador != null ?
                new ObjectParameter("identificador", identificador) :
                new ObjectParameter("identificador", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Activar", identificadorParameter);
        }
    
        public virtual int Desactivar(string identificador)
        {
            var identificadorParameter = identificador != null ?
                new ObjectParameter("identificador", identificador) :
                new ObjectParameter("identificador", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Desactivar", identificadorParameter);
        }
    
        public virtual int importar(string rutaArchivo)
        {
            var rutaArchivoParameter = rutaArchivo != null ?
                new ObjectParameter("RutaArchivo", rutaArchivo) :
                new ObjectParameter("RutaArchivo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("importar", rutaArchivoParameter);
        }
    
        public virtual ObjectResult<RAJ_Result> RAJ(string matricula, Nullable<System.DateTime> fechaI, Nullable<System.DateTime> fechaF)
        {
            var matriculaParameter = matricula != null ?
                new ObjectParameter("matricula", matricula) :
                new ObjectParameter("matricula", typeof(string));
    
            var fechaIParameter = fechaI.HasValue ?
                new ObjectParameter("fechaI", fechaI) :
                new ObjectParameter("fechaI", typeof(System.DateTime));
    
            var fechaFParameter = fechaF.HasValue ?
                new ObjectParameter("fechaF", fechaF) :
                new ObjectParameter("fechaF", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RAJ_Result>("RAJ", matriculaParameter, fechaIParameter, fechaFParameter);
        }
    
        public virtual ObjectResult<RCJ_Result> RCJ(Nullable<System.DateTime> fechaI, Nullable<System.DateTime> fechaF)
        {
            var fechaIParameter = fechaI.HasValue ?
                new ObjectParameter("fechaI", fechaI) :
                new ObjectParameter("fechaI", typeof(System.DateTime));
    
            var fechaFParameter = fechaF.HasValue ?
                new ObjectParameter("fechaF", fechaF) :
                new ObjectParameter("fechaF", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RCJ_Result>("RCJ", fechaIParameter, fechaFParameter);
        }
    
        public virtual ObjectResult<RGJ_Result> RGJ(Nullable<System.DateTime> fechaI, Nullable<System.DateTime> fechaF)
        {
            var fechaIParameter = fechaI.HasValue ?
                new ObjectParameter("fechaI", fechaI) :
                new ObjectParameter("fechaI", typeof(System.DateTime));
    
            var fechaFParameter = fechaF.HasValue ?
                new ObjectParameter("fechaF", fechaF) :
                new ObjectParameter("fechaF", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RGJ_Result>("RGJ", fechaIParameter, fechaFParameter);
        }
    
        public virtual ObjectResult<RMJ_Result> RMJ(Nullable<System.DateTime> fechaI, Nullable<System.DateTime> fechaF)
        {
            var fechaIParameter = fechaI.HasValue ?
                new ObjectParameter("fechaI", fechaI) :
                new ObjectParameter("fechaI", typeof(System.DateTime));
    
            var fechaFParameter = fechaF.HasValue ?
                new ObjectParameter("fechaF", fechaF) :
                new ObjectParameter("fechaF", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RMJ_Result>("RMJ", fechaIParameter, fechaFParameter);
        }
    }
}
