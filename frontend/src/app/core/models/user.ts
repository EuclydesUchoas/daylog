export interface User {
  id: string;
  name: string;
  email: string;
  profileId: number;
  profileName: string;
  companies: Company[];
  createdInfo: AuditInfo;
  updatedInfo: AuditInfo;
  deletedInfo?: DeletedInfo;
}

export interface Company {
  companyId: string;
  companyName: string;
  createdInfo: AuditInfo;
  updatedInfo: AuditInfo;
}

export interface AuditInfo {
  createdAt?: string;
  createdByUserId?: string;
  createdByUserName?: string;
  updatedAt?: string;
  updatedByUserId?: string;
  updatedByUserName?: string;
}

export interface DeletedInfo {
  isDeleted: boolean;
  deletedAt?: string;
  deletedByUserId?: string;
  deletedByUserName?: string;
}
