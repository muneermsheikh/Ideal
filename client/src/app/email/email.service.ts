import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { OrdersService } from '../orders/orders.service';
import { IClient, IClientOfficial } from '../shared/models/client';
import { IEmail } from '../shared/models/email';
import { EmailParams } from '../shared/models/emailParams';
import { IEnquiry } from '../shared/models/enquiry';
import { IPaginationEmail, PaginationEmail } from '../shared/models/paginationEmail';
import { ClientsService } from '../users/clients/clients.service';

@Injectable({
  providedIn: 'root'
})
export class EmailService {

  private baseUrl = environment.apiUrl;
  emails: IEmail[] = [];
  email: IEmail;
  pagination = new PaginationEmail();

  params = new EmailParams();
  errors: string[];

  totalCount: number;
  sortSelected = 'asc';
  sortOptions = [
    { name: 'Ascending by toSenderEmail', value: 'nameasc' },
    { name: 'Descending by toSenderEmail', value: 'nameDesc' },
    { name: 'Ascending by City Name', value: 'cityNameAsc' },
    { name: 'Descending by City Name', value: 'cityNameDesc' }
  ];


  constructor(private http: HttpClient,
              private custService: ClientsService,
              private enqService: OrdersService) { }

  addEmail(values: any): any {
    return this.http.post(this.baseUrl + 'Email', values).pipe(
      map((email: IEmail) => {
        if (email) {
          console.log(email);
        }
      }, error => {
        console.log(error);
      })
    );
  }


  updateEmail(values: IEmail): any {
    return this.http.put(this.baseUrl + 'Email', values).pipe(
      map((email: IEmail) => {
        if (email) {
          console.log('email updated'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }

    getDLAckEmail(id: number): any {
      /*
      const enq = this.enquiries.find(p => p.id === id);

      if (enq) {
      return of(enq);
    }
    */
      return this.http.get<IEmail>(this.baseUrl + 'Email/dlackn?emailId=' + id);
    }

    getEmails(useCache: boolean): any {
      if (useCache === false) {
        this.emails = [];
      }

      if (this.emails.length > 0 && useCache === true) {
        const pagesReceived = Math.ceil(this.emails.length / this.params.pageSize);

        if (this.params.pageNumber <= pagesReceived) {
          this.pagination.data =
            this.emails.slice((this.params.pageNumber - 1) * this.params.pageSize,
              this.params.pageNumber * this.params.pageSize);

          return of(this.pagination);
        }
      }

      let params = new HttpParams();

      if (this.params.senderEmailAddress){
        params = params.append('senderEmailAddress', this.params.senderEmailAddress);
      }

      if (this.params.toEmailAddress){
        params = params.append('toEmailAddress', this.params.toEmailAddress);
      }

      if (this.params.search)
      {
        params = params.append('search', this.params.search);
      }

      params = params.append('sort', this.params.sort);

      params = params.append('pageIndex', this.params.pageNumber.toString());
      params = params.append('pageSize', this.params.pageSize.toString());

      return this.http.get<IPaginationEmail>(this.baseUrl + 'email/emailindex',   // listwithstatus', // dlindex',
        { observe: 'response', params })
        .pipe(
          map(response => {
            this.emails = [...this.emails, ...response.body.data];
            this.pagination = response.body;
            return this.pagination;
          }, error => {
            console.log(error);
          })
        );
    }


    getParams(): EmailParams {
      return this.params;
    }

    setParams(params: EmailParams): void {
      this.params = params;
    }

    // this is duplicated here from Orders.Service - for testing purposes
    getEnquiry(id: number): any {
      return this.http.get<IEnquiry>(this.baseUrl + 'dl/getenquiry/' + id);
    }

    getClient(customerId: number): any {
      return this.http.get<IClient>(this.baseUrl + 'Customers/getcustomer/' + customerId);
    }

    getCustomerFromEnquiryId(enquiryId: number): any {
      return this.http.get<IClient>(this.baseUrl + 'Customers/custfromenquiryid/' + enquiryId);
    }
}
