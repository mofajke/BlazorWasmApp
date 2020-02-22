using System;
using System.Collections.Generic;
using System.Text;
using FluentMigrator;

namespace BlazorWasmApp.Migrations.Main
{
    [Migration(20191208220401)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE SCHEMA IF NOT EXISTS public;

                CREATE TABLE public.Users(
                   Id serial PRIMARY KEY,
                   Username VARCHAR (50) UNIQUE NOT NULL,
                   Password VARCHAR (4096) NOT NULL,
                   Email VARCHAR (355) UNIQUE NOT NULL,
                   CreatedDate date NOT NULL
                );
            ");
        }

        public override void Down()
        {
            
        }
    }
}
