import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { ProfessionService } from 'src/app/profession/profession.service';
import { IClient } from 'src/app/shared/models/client';
import { ClientParams } from 'src/app/shared/models/clientParams';
import { IIndustryType } from 'src/app/shared/models/industryType';
import { IPaginationClient, PaginationClient } from 'src/app/shared/models/paginationClient';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class ClientsService {
  private baseUrl = environment.apiUrl;
  clients: IClient[] = [];
  client: IClient;
  indTypes: IIndustryType[];
  pagination = new PaginationClient();

  params = new ClientParams();
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

  addClient(values: any): any {
    return this.http.post(this.baseUrl + 'Customers', values).pipe(
      map((cand: IClient) => {
        if (cand) {
          console.log(cand);
        }
      }, error => {
        console.log(error);
      })
    );
  }


  updateClient(values: IClient): any {
    return this.http.put(this.baseUrl + 'Customers', values).pipe(
      map((client: IClient) => {
        if (client) {
          console.log('client ' + client.customerName + ' updated'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }


  getClient(id: number): any {
    const client = this.clients.find(p => p.id === id);

    if (client) {
      console.log('clients.service.ts - found client in list of clients - id =' + client.city);
      return of(client);
    }
    // console.log('getting customer from api - id =' + id);
    return this.http.get<IClient>(this.baseUrl + 'Customers/getcustomer/' + id);
  }

  getCustomer(id: number): any {
    return this.http.get<IClient>(this.baseUrl + 'Customers/getcustomer/' + id);
  }

  getClients(useCache: boolean): any {

    if (useCache === false) {
      this.clients = [];
    }

    if (this.clients.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.clients.length / this.params.pageSize);

      if (this.params.pageNumber <= pagesReceived) {
        this.pagination.data =
          this.clients.slice((this.params.pageNumber - 1) * this.params.pageSize,
            this.params.pageNumber * this.params.pageSize);

        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.params.clientId !== 0) {
      params = params.append('clientId', this.params.clientId.toString());
    }

    if (this.params.clientName !== '') {
      params = params.append('clientName', this.params.clientName);
    }

    if (this.params.search)
    {
      params = params.append('search', this.params.search);
    }

    params = params.append('sort', this.params.sort);

    params = params.append('pageIndex', this.params.pageNumber.toString());
    params = params.append('pageSize', this.params.pageSize.toString());
    // console.log(params);

    return this.http.get<IPaginationClient>(this.baseUrl + 'Customers', { observe: 'response', params })
      .pipe(
        map(response => {
          this.clients = [...this.clients, ...response.body.data];
          // console.log(this.clients);
          this.pagination = response.body;
          return this.pagination;
        }, error => {
          console.log(error);
        })
      );
  }

  getClientParams(): ClientParams {
    return this.params;
  }

  setClientParams(params: ClientParams): void {
    this.params = params;
  }

  getIndustryTypes(): any {
    return this.http.get<IIndustryType[]>(this.baseUrl + 'Category/IndustryTypesWoPagination');
  }

}
