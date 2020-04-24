using BlogIndex.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogIndex.Models
{
    public class BloggingContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory
                                 = LoggerFactory.Create(builder =>
                                 {
                                     builder
                                        .AddFilter((category, level) =>
                                            category == DbLoggerCategory.Database.Command.Name
                                            && level == LogLevel.Information)
                                        .AddConsole();
                                 });
        public BloggingContext(DbContextOptions<BloggingContext> options)
        : base(options)
        { }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Base_Attribute> BaseAttributes { get; set; }
        public DbSet<Mx_Question> MxQuestions { get; set; }
        public DbSet<Mx_QuestionCategory> MxQuestionCategories { get; set; }
        public DbSet<Mx_Attribute> MxAttributes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLoggerFactory(MyLoggerFactory) // Warning: Do not create a new ILoggerFactory instance each time
                .UseSqlServer(
                    //@"Server=(localdb)\\mssqllocaldb;Database=Blogging;Trusted_Connection=True;"
                    @"Data Source=.;Initial Catalog=Blogging;Integrated Security=True"
                    , options => options.EnableRetryOnFailure());
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(e =>
            {
                e.HasKey(b => b.Id);
                e.Property(b => b.Id).ValueGeneratedOnAdd();
                e.Property(b => b.Url).IsRequired();
                e.Property(b => b.IsDeleted).HasDefaultValue<bool>(false);//设置默认值
                e.Property(b => b.Name).HasComputedColumnSql("[FamilyName]+[LastName]");//关系型数据库计算其值
                e.Property(b => b.LastName).IsConcurrencyToken();//并发标记控制
                e.Property(b => b.Timestamp).IsRowVersion();//在每次插入或更新行时，数据库会自动为其生成新值.确认数据操作的并发唯一性
                //e.HasData(new Blog { FamilyName = "焦", LastName = "豪杰", Url = "http://baidu.com" });//数据种子
            });
            modelBuilder.Entity<Base_Attribute>(e =>
            {
                e.HasKey(b => b.Id);
                e.Property(b => b.Id).ValueGeneratedOnAdd();
                e.HasDiscriminator<string>("Discriminator")//鉴别器 HasDiscriminator(b=>b.Discriminator)
                 .HasValue<Base_Attribute>("Base")
                 .HasValue<Mx_Attribute>("Mx");
                //e.Property(b => b.AttributeName).HasColumnName("AttributeName");//共享列 列名相同则共享列
            });
            modelBuilder.Entity<Mx_Attribute>(e =>
            {
                //e.Property(b => b.AttributeName).HasColumnName("AttributeName");//共享列 列名相同则共享列
            });
            modelBuilder.Entity<Mx_Question>(e =>
            {
                e.HasOne(b => b.Mx_QuestionCategory).WithMany(p => p.Mx_Questions)//一对一 对应单条类别  反向导航 类别对应多条题目
                 .HasForeignKey(b=>b.Mx_QuestionCategoryId)//外键设置
                 //.IsRequired() //必选关系
                 .OnDelete(DeleteBehavior.ClientSetNull)//级联删除，对于可选关系设置 导航属性若删除 则外键设置为null
                 ;
                e.Property(b => b.QuestionType).HasConversion<string>();//内置枚举值转换
            });
           
        }
    }
}
