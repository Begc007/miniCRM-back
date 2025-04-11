﻿namespace miniCRM_back.Models {
    public class User:BaseEntity {
        public required string Name { get; set; }
        public required string PasswordHash { get; set; }
        public string? FIO { get; set; } //TODO: check if the field can be nullable in ТЗ
        public string? Position { get; set; } //TODO: check if the field can be nullable in ТЗ
        public List<TaskItem>? Tasks {
            get;
            set;
        }
    }
}
