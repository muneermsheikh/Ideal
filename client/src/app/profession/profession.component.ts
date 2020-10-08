import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IProfession } from '../shared/models/profession';
import { ProfParams } from '../shared/models/profParams';
import { ProfessionService } from './profession.service';

@Component({
  selector: 'app-profession',
  templateUrl: './profession.component.html',
  styleUrls: ['./profession.component.scss']
})

export class ProfessionComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  professions: IProfession[];

  profParams = new ProfParams();
  totalCount: number;
  sortSelected = 'asc';
  sortOptions = [
    { name: 'Ascending by category', value: 'categoryNameasc' },
    { name: 'Descending by category', value: 'categoryNameDesc' },
    { name: 'Ascending by Industry Type', value: 'industryTypeAsc' },
    { name: 'Descending by Industry Type', value: 'industryTypeDesc' },
    { name: 'Ascending by skill Level', value: 'skillLevelAsc' },
    { name: 'Ascending by skill level', value: 'skillLevelDesc' }
  ];

  constructor(private profService: ProfessionService) {
    this.profParams = this.profService.getProfParams();
   }

  ngOnInit(): void {
    this.getProfessions();
  }

  getProfessions(): any {
    this.profService.getProfessions(this.profParams).subscribe(response => {
      this.professions = response.data;
      this.totalCount = response.count;
      this.profParams.pageNumber = response.pageIndex;
      this.profParams.pageSize = response.pageSize;
    }, error => {
      console.log(error);
    });
  }


  onPageChanged(event: any): void {
    const params = this.profService.getProfParams();
    // this.shopParams.pageNumber = event;
    if (params.pageNumber !== event) {
      params.pageNumber = event;
//      this.shopService.setShopParams(params);
      this.getProfessions();
    }
  }

  onSearch(): void{
    console.log(this.searchTerm.nativeElement.value);
    const params = this.profService.getProfParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.profService.setProfParams(params);

    this.getProfessions();
  }


  onReset(): any {
    this.searchTerm.nativeElement.value = '';
    this.profParams = new ProfParams();
    // this.shopService.setShopParams(this.shopParams);
    this.getProfessions();
  }

  onSortSelected(sort: string): any {
    const params = this.profService.getProfParams();
    params.sort = sort;
    this.profService.setProfParams(params);
    this.getProfessions();
  }

}
