import { Injectable } from '@angular/core';
import { EmailGroup } from '../models/email-group.model';

@Injectable()
export class MailToService {

    public getMailto(emailGroup: EmailGroup): string {
        if (emailGroup.emails == null || emailGroup.emails.length === 0) {
            return '';
        }

        const emailsJoined = emailGroup.emails.map(item => item.address).join(',');
        return `mailto:${emailsJoined}`;
    }

}
