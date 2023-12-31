﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SIERRA_Server.Models.EFModels
{
    public partial class Lesson
    {
        public Lesson()
        {
            LessonImages = new HashSet<LessonImage>();
            LessonOrderDetails = new HashSet<LessonOrderDetail>();
        }

        public int LessonId { get; set; }
        public int LessonCategoryId { get; set; }
        public int TeacherId { get; set; }
        public string LessonTitle { get; set; }
        public string LessonInfo { get; set; }
        public string LessonDetail { get; set; }
        public string LessonDessert { get; set; }
        public DateTime LessonTime { get; set; }
        public int LessonHours { get; set; }
        public int MaximumCapacity { get; set; }
        public int LessonPrice { get; set; }
        public bool LessonStatus { get; set; }
        public int ActualCapacity { get; set; }
        public DateTime LessonEndTime { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual LessonCategory LessonCategory { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<LessonImage> LessonImages { get; set; }
        public virtual ICollection<LessonOrderDetail> LessonOrderDetails { get; set; }
    }
}