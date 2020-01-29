import { EmailGroup } from './../models/email-group.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { retry, catchError, first, map, delay } from 'rxjs/operators';
import { throwError, Observable, of, empty } from 'rxjs';
import { BackendEmailGroup, NewBackendEmailGroup } from './backend.models';
import { Email } from '../models/email.model';
import { MailToService } from './mailto.service';

@Injectable()
export class BackendService {
    private readonly primaryUrl = 'http://localhost:58210/api/groups';

    constructor(private httpClient: HttpClient, private mailtoService: MailToService) { }

    public async getGroups(): Promise<EmailGroup[]> {
        return await this.httpClient.get<BackendEmailGroup[]>(this.primaryUrl)
            .pipe(retry(2),
                catchError(this.handleError),
                delay(500),
                first(),
                map(group => group.map(val => this.MapToEmailGroup(val))))
            .toPromise();
    }

    public async addGroups(newGroup: EmailGroup): Promise<EmailGroup> {
        const backendModel = this.MapToBackendEmailGroup(newGroup);

        return await this.httpClient.post<BackendEmailGroup>(this.primaryUrl, backendModel)
            .pipe(retry(2),
                catchError(this.handleError),
                first(),
                map(val => this.MapToEmailGroup(val)))
            .toPromise();
    }

    public async updateGroup(group: EmailGroup): Promise<EmailGroup> {
        const backendModel = this.MapToBackendEmailGroup(group);

        const url = `${this.primaryUrl}/${group.id}`;
        return await this.httpClient.put<BackendEmailGroup>(url, backendModel)
            .pipe(retry(2),
                catchError(this.handleError),
                first(),
                map(val => this.MapToEmailGroup(val)))
            .toPromise();
    }

    public async deleteGroup(groupId: number): Promise<any> {
        const url = `${this.primaryUrl}/${groupId}`;
        return await this.httpClient.delete(url)
            .pipe(retry(2),
                catchError(this.handleError),
                first())
            .toPromise();
    }

    private MapToEmailGroup(backendGroup: BackendEmailGroup): EmailGroup {
        const result = {
            id: backendGroup.id,
            name: backendGroup.name,
            emails: backendGroup.emails.map(item => {
                return { address: item } as Email;
            }),
        } as EmailGroup;

        result.mailto = this.mailtoService.getMailto(result);

        return result;
    }

    private MapToBackendEmailGroup(group: EmailGroup): NewBackendEmailGroup {
        return {
            name: group.name,
            emails: group.emails.map(val => val.address)
        } as NewBackendEmailGroup;
    }

    private handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            console.error('An error occurred:', error.error.message);
        } else {
            console.error(
                `Backend returned code ${error.status}, ` +
                `body was: ${error.error}`);
        }

        return throwError('Failed to send backend request.');
    }
}
