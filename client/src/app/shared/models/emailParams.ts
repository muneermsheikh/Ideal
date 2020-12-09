export class EmailParams {
    senderName: string;
    senderEmailAddress: string;
    toEmailAddress: string;
    subject: boolean;
    sort = 'asc';
    pageNumber = 1;
    pageSize = 6;
    search: string;
}
