-- 1. TABELAS DO MICROSOFT IDENTITY
CREATE TABLE "AspNetRoles" (
    "Id" TEXT NOT NULL PRIMARY KEY,
    "Name" VARCHAR(256),
    "NormalizedName" VARCHAR(256),
    "ConcurrencyStamp" TEXT
);

CREATE TABLE "AspNetUsers" (
    "Id" TEXT NOT NULL PRIMARY KEY,
    "UserName" VARCHAR(256),
    "NormalizedUserName" VARCHAR(256),
    "Email" VARCHAR(256),
    "NormalizedEmail" VARCHAR(256),
    "EmailConfirmed" BOOLEAN NOT NULL,
    "PasswordHash" TEXT,
    "SecurityStamp" TEXT,
    "ConcurrencyStamp" TEXT,
    "PhoneNumber" TEXT,
    "PhoneNumberConfirmed" BOOLEAN NOT NULL,
    "TwoFactorEnabled" BOOLEAN NOT NULL,
    "LockoutEnd" TIMESTAMPTZ,
    "LockoutEnabled" BOOLEAN NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL
);

-- 2. TABELAS DE DOMÍNIO (GYMNARTE)

CREATE TABLE "UserScopes" (
    "UserScopeId" SERIAL PRIMARY KEY,
    "Role" INTEGER NOT NULL, -- Enum UserRole
    "CreationDate" TIMESTAMPTZ NOT NULL,
    "UpdatedDate" TIMESTAMPTZ,
    "UserId" INTEGER NOT NULL,
    "UpdatedUserId" INTEGER,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE "Users" (
    "UserId" SERIAL PRIMARY KEY,
    "PartnerNumber" INTEGER NOT NULL,
    "UserName" VARCHAR(256) NOT NULL,
    "Email" VARCHAR(256) NOT NULL,
    "Password" TEXT NOT NULL,
    "BirthDate" TIMESTAMPTZ NOT NULL,
    "Gender" INTEGER, -- Enum Gender
    "Name" VARCHAR(256) NOT NULL,
    "Surname" VARCHAR(256) NOT NULL,
    "Phone" VARCHAR(50),
    "CreationDate" TIMESTAMPTZ NOT NULL,
    "UpdatedDate" TIMESTAMPTZ,
    "UpdatedUserId" INTEGER REFERENCES "Users" ("UserId"),
    "UserScopeId" INTEGER NOT NULL REFERENCES "UserScopes" ("UserScopeId")
);

-- Ajuste da FK circular de UserScopes para Users
ALTER TABLE "UserScopes" ADD CONSTRAINT "FK_UserScopes_Users_UserId" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("UserId");
ALTER TABLE "UserScopes" ADD CONSTRAINT "FK_UserScopes_Users_UpdatedUserId" 
    FOREIGN KEY ("UpdatedUserId") REFERENCES "Users" ("UserId");

CREATE TABLE "ExerciseTypes" (
    "ExerciseTypeId" SERIAL PRIMARY KEY,
    "Name" VARCHAR(256) NOT NULL,
    "Description" TEXT,
    "CreationDate" TIMESTAMPTZ NOT NULL,
    "UpdatedDate" TIMESTAMPTZ,
    "UpdatedUserId" INTEGER REFERENCES "Users" ("UserId")
);

CREATE TABLE "Exercises" (
    "ExerciseId" SERIAL PRIMARY KEY,
    "Name" VARCHAR(256) NOT NULL,
    "Notes" TEXT,
    "CreationDate" TIMESTAMPTZ NOT NULL,
    "UpdatedDate" TIMESTAMPTZ,
    "ExerciseTypeId" INTEGER NOT NULL REFERENCES "ExerciseTypes" ("ExerciseTypeId"),
    "UpdatedUserId" INTEGER REFERENCES "Users" ("UserId")
);

CREATE TABLE "TrainingPlans" (
    "TrainingPlanId" SERIAL PRIMARY KEY,
    "TrainingPlanName" VARCHAR(256) NOT NULL,
    "Notes" TEXT,
    "CreationDate" TIMESTAMPTZ NOT NULL,
    "UpdatedDate" TIMESTAMPTZ,
    "UserId" INTEGER NOT NULL REFERENCES "Users" ("UserId"),
    "ExerciseTypeId" INTEGER NOT NULL REFERENCES "ExerciseTypes" ("ExerciseTypeId"),
    "UpdatedUserId" INTEGER REFERENCES "Users" ("UserId")
);

CREATE TABLE "ExerciseTrainingPlans" (
    "ExerciseTrainingPlanId" SERIAL PRIMARY KEY,
    "Sets" INTEGER NOT NULL,
    "Reps" INTEGER NOT NULL,
    "Notes" TEXT,
    "ExerciseId" INTEGER NOT NULL REFERENCES "Exercises" ("ExerciseId"),
    "TrainingPlanId" INTEGER NOT NULL REFERENCES "TrainingPlans" ("TrainingPlanId") ON DELETE CASCADE,
    "IsActive" BOOLEAN NOT NULL
);

CREATE TABLE "BiometricData" (
    "BiometricDataId" SERIAL PRIMARY KEY,
    "RecordDate" TIMESTAMPTZ NOT NULL,
    "Weight" DECIMAL(5,2) NOT NULL,
    "Height" DECIMAL(5,2) NOT NULL,
    "Age" INTEGER NOT NULL,
    "FatPercent" DECIMAL(5,2) NOT NULL,
    "LeanMassPercent" DECIMAL(5,2) NOT NULL,
    "BodyWaterPercent" DECIMAL(5,2) NOT NULL,
    "VisceralFat" DECIMAL(5,2) NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "UserId" INTEGER NOT NULL REFERENCES "Users" ("UserId")
);

CREATE TABLE "Notifications" (
    "NotificationId" SERIAL PRIMARY KEY,
    "Description" TEXT NOT NULL,
    "Read" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreationDate" TIMESTAMPTZ NOT NULL,
    "UserId" INTEGER NOT NULL REFERENCES "Users" ("UserId")
);