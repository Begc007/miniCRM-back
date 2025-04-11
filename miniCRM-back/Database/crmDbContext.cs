﻿using Microsoft.EntityFrameworkCore;
using miniCRM_back.Models;

namespace miniCRM_back.Database {
    public class crmDbContext:DbContext {
        public crmDbContext(DbContextOptions<crmDbContext> options):base(options) {}
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
