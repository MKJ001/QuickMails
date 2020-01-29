export interface BackendEmailGroup {
    id: number;
    name: string;
    emails: string[];
}

export interface NewBackendEmailGroup {
    name: string;
    emails: string[];
}
