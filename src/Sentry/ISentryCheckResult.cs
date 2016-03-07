﻿using System;

namespace Sentry
{
    public interface ISentryCheckResult : ITimestampable
    {
        IWatcherCheckResult WatcherCheckResult { get; }
        Exception Exception { get; }
        bool IsValid { get; }
    }

    public class SentryCheckResult : ISentryCheckResult
    {
        public IWatcherCheckResult WatcherCheckResult { get; }
        public DateTime StartedAtUtc { get; }
        public DateTime CompletedAtUtc { get; }
        public Exception Exception { get; }
        public TimeSpan ExecutionTime => CompletedAtUtc - StartedAtUtc;
        public bool IsValid => Exception == null;

        protected SentryCheckResult(IWatcherCheckResult watcherCheckResult, DateTime startedAtUtc, DateTime completedAtUtc,
            Exception exception = null)
        {
            if (watcherCheckResult == null)
                throw new ArgumentNullException(nameof(watcherCheckResult), "Watcher check result can not be null.");

            WatcherCheckResult = watcherCheckResult;
            StartedAtUtc = startedAtUtc;
            CompletedAtUtc = completedAtUtc;
            Exception = exception;
        }

        public static SentryCheckResult Valid(IWatcherCheckResult watcherCheckResult, DateTime startedAt,
            DateTime completedAt) => Create(watcherCheckResult, startedAt, completedAt);

        public static SentryCheckResult Invalid(IWatcherCheckResult watcherCheckResult, DateTime startedAt,
            DateTime completedAt, Exception exception)
            => Create(watcherCheckResult, startedAt, completedAt, exception);

        private static SentryCheckResult Create(IWatcherCheckResult watcherCheckResult, DateTime startedAt,
            DateTime completedAt, Exception exception = null)
            => new SentryCheckResult(watcherCheckResult, startedAt, completedAt, exception);
    }
}