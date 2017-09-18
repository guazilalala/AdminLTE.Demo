using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AdminLTE.Demo.Domain.Entities
{
    /// <summary>
    /// 业务字典实体
    /// </summary>
   public class DataDictionary:Entity
    {
        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [StringLength(50)]
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(20)]
        public string Code { get; set; }


        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
