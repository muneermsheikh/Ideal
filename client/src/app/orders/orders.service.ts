import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ProfessionService } from '../profession/profession.service';
import { IClient, IClientOfficial } from '../shared/models/client';
import { IEnquiry } from '../shared/models/enquiry';
import { EnquiryParams } from '../shared/models/enquiryParams';
import { IPaginationEnquiry, PaginationEnquiry } from '../shared/models/paginationEnquiry';
import { IProfession } from '../shared/models/profession';

@Injectable({
  providedIn: 'root'
})

export class OrdersService {
  private baseUrl = environment.apiUrl;
  enquiries: IEnquiry[] = [];
  enquiry: IEnquiry;
  professions: IProfession[];
  allOfficials: IClientOfficial[] = [];  // all officials of all customers
  officials: IClientOfficial[];   // officials of a customer
  clients: IClient[];
  pagination = new PaginationEnquiry();

  params = new EnquiryParams();
  errors: string[];

  totalCount: number;
  sortSelected = 'asc';
  sortOptions = [
    { name: 'Ascending by name', value: 'nameasc' },
    { name: 'Descending by name', value: 'nameDesc' },
    { name: 'Ascending by City Name', value: 'cityNameAsc' },
    { name: 'Descending by City Name', value: 'cityNameDesc' }
  ];


  constructor(private http: HttpClient,
              private profService: ProfessionService) { }

  addEnquiry(values: any): any {
    // console.log('in service - ' + values);
    return this.http.post(this.baseUrl + 'DL', values).pipe(
      map((enq: IEnquiry) => {
        if (enq) {
          console.log(enq);
        }
      }, error => {
        console.log(error);
      })
    );
  }


  updateEnquiry(values: IEnquiry): any {
    return this.http.put(this.baseUrl + 'DL', values).pipe(
      map((enq: IEnquiry) => {
        if (enq) {
          console.log('Enquiry No ' + enq.enquiryNo + ' updated'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }

    getEnquiry(id: number): any {
      /*
      const enq = this.enquiries.find(p => p.id === id);

      if (enq) {
      return of(enq);
    }
    */
      return this.http.get<IEnquiry>(this.baseUrl + 'DL/getenquiry/' + id);
    }

    getEnquiries(useCache: boolean): any {
      if (useCache === false) {
        this.enquiries = [];
      }

      if (this.enquiries.length > 0 && useCache === true) {
        const pagesReceived = Math.ceil(this.enquiries.length / this.params.pageSize);

        if (this.params.pageNumber <= pagesReceived) {
          this.pagination.data =
            this.enquiries.slice((this.params.pageNumber - 1) * this.params.pageSize,
              this.params.pageNumber * this.params.pageSize);

          return of(this.pagination);
        }
      }

      let params = new HttpParams();

      if (this.params.enquiryId !== 0) {
        params = params.append('enquiryId', this.params.enquiryId.toString());
      }

      if (this.params.enquiryNo !== 0) {
        params = params.append('enquiryNo', this.params.enquiryNo.toString());
      }

      if (this.params.search)
      {
        params = params.append('search', this.params.search);
      }

      params = params.append('sort', this.params.sort);

      params = params.append('pageIndex', this.params.pageNumber.toString());
      params = params.append('pageSize', this.params.pageSize.toString());

      return this.http.get<IPaginationEnquiry>(this.baseUrl + 'DL/dlindex',   // listwithstatus', // dlindex',
        { observe: 'response', params })
        .pipe(
          map(response => {
            this.enquiries = [...this.enquiries, ...response.body.data];
            this.pagination = response.body;
            return this.pagination;
          }, error => {
            console.log(error);
          })
        );
    }


    getAllOfficials(): any {
      return this.http.get<IClientOfficial[]>(this.baseUrl + 'Customers/officials', {observe: 'response'})
      .pipe(
        map(response => {
          this.allOfficials = response.body;
          return response.body;
        })
      );
    }

    getClientOfficials(id: number): any {
      const offs = this.allOfficials.find(p => p.customerId === id);
      if (offs) {
        return of(offs);
      }

      return  this.http.get<IClientOfficial[]>(this.baseUrl + 'Customers/officials/' + id);
    }

    getCustomerOfficials(id: number): any {
      const off = this.allOfficials.find(x => x.customerId === id);
      if (off) {
        console.log(' got the customer officials from all officials');
        return of(off);
      }
      console.log('failed to get officials from all officials, getting from API');
      return this.http.get(this.baseUrl + 'Customers/officials/' + id);
    }


    getClients(): any {
      return this.http.get(this.baseUrl + 'customers/customersflat/customer');
    }

    getEnquiryParams(): EnquiryParams {
      return this.params;
    }

    setEnquiryParams(params: EnquiryParams): void {
      this.params = params;
    }


}
