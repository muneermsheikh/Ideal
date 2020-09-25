import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ICategory } from '../shared/models/category';
import { IIndustryType } from '../shared/models/industryType';
import { ShopParams } from '../shared/models/shopParams';
import { ISkillLevel } from '../shared/models/skillLevel';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})

export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm: ElementRef;
  categories: ICategory[];
  indtypes: IIndustryType[];
  skilllevels: ISkillLevel[];
  indtypeidSelected = 0;
  skilllevelidSelected = 0;

  shopParams = new ShopParams();
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

  constructor(private shopService: ShopService) {
    this.shopParams = this.shopService.getShopParams();
   }

  ngOnInit(): void {
    this.getCategories();
    this.getIndustryTypes();
    this.getSkillLevels();
  }

  /*
  getCategories(): any {
    this.shopService.getCategories(this.indtypeidSelected, this.skilllevelidSelected).subscribe(response =>
      {this.categories = response.data; },
      error => {console.log(error); } );
  }
*/

  getCategories(): any {
    this.shopService.getCategories(this.shopParams).subscribe(response => {
    this.categories = response.data;
    this.totalCount = response.count;
    this.shopParams.pageNumber = response.pageNumber;
    this.shopParams.pageSize = response.pageSize;
  }, error => {
    console.log(error);
  });
}

  getIndustryTypes(): any {
    this.shopService.getIndustryTypes().subscribe(response =>
      {this.indtypes = [{id: 0, name: 'All'}, ...response]; },
      error => {console.log(error); } );
  }

  getSkillLevels(): any {
    this.shopService.getSkillLevels().subscribe(response =>
      {this.skilllevels = [{id: 0, name : 'All'}, ...response]; },
      error => {console.log(error); } );
  }

  onIndTypeSelected(indTypeId: number): any {
    // const params = this.shopService.getShopParams();
    this.shopParams.industryTypeId = indTypeId;
    this.shopParams.pageNumber = 1;
    this.shopService.setShopParams(this.shopParams);
    this.getCategories();
  }

  onSkillLevelSelected(id: number): any {
    // const params = this.shopService.getShopParams();
    this.shopParams.skillLevelId = id;
    this.shopParams.pageNumber = 1;
    this.shopService.setShopParams(this.shopParams);
    this.getCategories();
  }

  onPageChanged(event: any) {
    // const params = this.shopService.getShopParams();
    this.shopParams.pageNumber = event.page;
    // if (params.pageNumber !== event) {
    // params.pageNumber = event;
//      this.shopService.setShopParams(params);
    this.getCategories();
    // }
  }

  onSearch(): any {
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getCategories();
  }

  onReset(): any {
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.shopService.setShopParams(this.shopParams);
    this.getCategories();
  }

  onSortSelected(sort: string): any {
    const params = this.shopService.getShopParams();
    params.sort = sort;
    this.shopService.setShopParams(params);
    this.getCategories();
  }
}
