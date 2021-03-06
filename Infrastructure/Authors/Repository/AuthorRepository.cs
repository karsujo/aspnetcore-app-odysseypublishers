﻿using Application.Authors;
using AutoMapper;
using Dapper;
using Infrastructure.Authors;
using OdysseyPublishers.Application.Authors;
using OdysseyPublishers.Application.Common;
using OdysseyPublishers.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OdysseyPublishers.Infrastructure.Authors
{

    public class AuthorRepository : IAuthorRepository
    {

        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public AuthorRepository(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public IEnumerable<Author> GetAuthors()
        {

            //Get all authors, and for each author get books
            string sql = @" SELECT *
            FROM
              AUTHORS ";

            var result = _repository.QueryDatabase<AuthorDbEntity>(sql, null);
            return _mapper.Map<IEnumerable<Author>>(result);
        }

        public IEnumerable<Author> GetAuthors(AuthorResourceParameters resourceParameters)
        {

            //Get all authors, and for each author get books
            if (string.IsNullOrEmpty(resourceParameters.State) && string.IsNullOrEmpty(resourceParameters.City))
            {
                return GetAuthors();
            }

            StringBuilder sql = new StringBuilder(@" SELECT *
            FROM
              AUTHORS ");
            var parameters = new DynamicParameters();
            bool isStateFilterAdded = false;
            if (!string.IsNullOrEmpty(resourceParameters.State))
            {
                isStateFilterAdded = true;
                sql.Append(@" WHERE State = @State");
                parameters.Add("@State", resourceParameters.State, DbType.String, ParameterDirection.Input, resourceParameters.State.Length);
            }
            if (!string.IsNullOrEmpty(resourceParameters.City))
            {
                if (isStateFilterAdded)
                {
                    sql.Append(@" AND City = @City");
                }
                else
                {
                    sql.Append(@" WHERE City = @City");
                }
                parameters.Add("@City", resourceParameters.City.Trim(), DbType.String, ParameterDirection.Input, resourceParameters.City.Length);
            }
            var result = _repository.QueryDatabase<AuthorDbEntity>(sql.ToString(), parameters);
            return _mapper.Map<IEnumerable<Author>>(result);
        }
        public Author GetAuthor(string authorId)
        {
            string sql = "select* from authors where au_id = @AuthorId";
            var parameters = new DynamicParameters();
            parameters.Add("@AuthorId", authorId, DbType.String, ParameterDirection.Input, authorId.Length);
            var result = _repository.QueryDatabase<AuthorDbEntity>(sql, parameters).FirstOrDefault();
            return _mapper.Map<Author>(result);
        }

        public bool AuthorExists(string authorId)
        {
            string sql = "select* from authors where au_id = @AuthorId";
            var parameters = new DynamicParameters();
            parameters.Add("@AuthorId", authorId, DbType.String, ParameterDirection.Input, authorId.Length);
            var result = _repository.QueryDatabase<AuthorDbEntity>(sql, parameters).FirstOrDefault();

            return result == null ? false : true;
        }

        public void CreateAuthor(AuthorForCreationDto authorForCreationDto, string authorId)
        {
            string sql = @"insert into authors( au_id,
            au_lname ,
            au_fname ,
            phone   ,
            address ,
            city   ,
            state  ,
            zip) 
          values( @au_id,
            @au_lname ,
            @au_fname ,
            @phone   ,
            @address ,
            @city   ,
            @state  ,
            @zip )";
            var parameters = new DynamicParameters();
            //TODO: Remove unnecessary params
            parameters.Add("@au_id", authorId, DbType.String, ParameterDirection.Input, authorId.Length);
            parameters.Add("@au_fname", authorForCreationDto.LastName, DbType.String, ParameterDirection.Input, authorForCreationDto.LastName.Length);
            parameters.Add("@au_lname", authorForCreationDto.FirstName, DbType.String, ParameterDirection.Input, authorForCreationDto.FirstName.Length);
            parameters.Add("@phone", authorForCreationDto.Phone, DbType.String, ParameterDirection.Input, authorForCreationDto.Phone.Length);
            parameters.Add("@address", authorForCreationDto.Address, DbType.String, ParameterDirection.Input, authorForCreationDto.Address.Length);
            parameters.Add("@city", authorForCreationDto.City, DbType.String, ParameterDirection.Input, authorForCreationDto.City.Length);
            parameters.Add("@state", authorForCreationDto.State, DbType.String, ParameterDirection.Input, authorForCreationDto.State.Length);
            parameters.Add("@zip", authorForCreationDto.Zip, DbType.String, ParameterDirection.Input, authorForCreationDto.Zip.Length);
            _repository.ModifyDatabase(sql, parameters);

        }



        public void UpdateAuthor(AuthorForUpdateDto author)
        {
            string sql = @"Update authors set au_fname = @au_fname, au_lname= @au_lname, phone = @phone, address= @address, city = @city, state = @state, zip = @zip where au_id= @au_id";
            var parameters = new DynamicParameters();
            parameters.Add("@au_id", author.AuthorId, DbType.String, ParameterDirection.Input);
            parameters.Add("@au_fname", author.FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("@au_lname", author.LastName, DbType.String, ParameterDirection.Input);
            parameters.Add("@phone", author.Phone, DbType.String, ParameterDirection.Input);
            parameters.Add("@address", author.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@city", author.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@state", author.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@zip", author.Zip, DbType.String, ParameterDirection.Input);
            _repository.ModifyDatabase(sql, parameters);

        }


        public void DeleteAuthor(string authorId)
        {
            string sql = @"Delete from authors where au_id = @au_id";
            var parameters = new DynamicParameters();
            parameters.Add("@au_id", authorId, DbType.String, ParameterDirection.Input);

            _repository.ModifyDatabase(sql, parameters);
        }
    }

}
