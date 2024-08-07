using DesigneryCore.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesigneryCommon.Models;
using Npgsql;

namespace DesigneryCore.Services
{
    public class MessageService : IMessageService
    {
        private readonly string _connectionString;

        public MessageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgreSqlConnection");
        }

        /*public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync()
        {
            var messages = new List<MessageDto>();
            string query = @"
        SELECT m.Id AS MessageId, m.Message, d.Id AS DataEntryId, d.Name, d.Email
        FROM Messages m
        INNER JOIN DataEntries d ON m.DataEntryId = d.Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            messages.Add(new MessageDto
                            {
                                MessageId = (int)reader["MessageId"],
                                Message = reader["Message"].ToString(),
                                DataEntryId = (int)reader["DataEntryId"],
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString()
                            });
                        }
                    }
                }
            }

            return messages;
        }*/
        
        
        public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync()
        {
            var messages = new List<MessageDto>();
            string query = "SELECT * FROM get_all_messages();";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            messages.Add(new MessageDto
                            {
                                MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                                Message = reader.GetString(reader.GetOrdinal("Message")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email"))
                            });
                        }
                    }
                }
            }

            return messages;
        }



        /*public async Task<MessageDto> GetMessageByIdAsync(int messageId)
            {
                string query = @"
            SELECT m.Message, m.DataEntryId, d.Name, d.Email
            FROM Messages m
            INNER JOIN DataEntries d ON m.DataEntryId = d.Id
            WHERE m.Id = @MessageId";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MessageId", messageId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new MessageDto
                                {
                                    Message = reader["Message"].ToString(),
                                    DataEntryId = (int)reader["DataEntryId"],
                                    Name = reader["Name"].ToString(),
                                    Email = reader["Email"].ToString()
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }*/

        public async Task<MessageDto> GetMessageByIdAsync(int messageId)
        {
            string query = "SELECT * FROM get_message_by_id(@MessageId);";
            var parameters = new[]
            {
                   new NpgsqlParameter("@MessageId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = messageId }
            };
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new MessageDto
                            {
                                Message = reader.GetString(reader.GetOrdinal("Message")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email"))
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }


        /*public async Task<bool> DeleteMessageAsync(int messageId)
        {
            string query = "DELETE FROM Messages WHERE Id = @MessageId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MessageId", messageId);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }*/
        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            string query = "DELETE FROM messages WHERE id = @MessageId";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MessageId", NpgsqlTypes.NpgsqlDbType.Integer, messageId);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

    }
}
