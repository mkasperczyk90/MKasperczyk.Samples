﻿using MKasperczyk.Chat.Api.DAL;

namespace MKasperczyk.Chat.Api.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private ChatContext _context;
        private IUserRepository _userRepository;
        private IMessageRepository _messageRepository;
        public UnitOfWork(ChatContext chatContext)
        {
            _context = chatContext;
            _userRepository = new UserRepository(_context);
            _messageRepository = new MessageRepository(_context);
        }

        public IUserRepository UserRepository
        {
            get
            {

                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }
        public IMessageRepository MessageRepository
        {
            get
            {

                if (_messageRepository == null)
                {
                    _messageRepository = new MessageRepository(_context);
                }
                return _messageRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
