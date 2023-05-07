using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ray.Infrastructure.QingLong
{
    public interface IQingLongApi
    {
        [Get("/api/envs")]
        Task<QingLongGenericResponse<List<QingLongEnv>>> GetEnvsAsync(string searchValue);

        [Post("/api/envs")]
        Task<QingLongGenericResponse<List<QingLongEnv>>> AddEnvsAsync([Body] List<AddQingLongEnv> envs);

        [Put("/api/envs")]
        Task<QingLongGenericResponse<QingLongEnv>> UpdateEnvsAsync([Body] UpdateQingLongEnv env);
    }


    public class QingLongGenericResponse<T>
    {
        public int Code { get; set; }

        public T Data { get; set; }
    }


    public class QingLongEnv : UpdateQingLongEnv
    {
        public string timestamp { get; set; }
        public int status { get; set; }
        //public long position { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class AddQingLongEnv
    {
        public string value { get; set; }
        public string name { get; set; }
        public string remarks { get; set; }
    }

    public class UpdateQingLongEnv : AddQingLongEnv
    {
        public long id { get; set; }
    }
}
