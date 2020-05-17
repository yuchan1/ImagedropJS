using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models {
    [Table("T_FileUploads")]
    public class FileUpload {

        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "ファイル名")]
        public string FileName { get; set; }

        [StringLength(255, ErrorMessage = "{0} の長さは {1} 文字以内で入力して下さい。")]
        [Display(Name = "備考")]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "順序")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "非表示")]
        public bool IsDeleted { get; set; }

        [Display(Name = "作成日時")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "更新日時")]
        public DateTime? UpdatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation property

    }
}