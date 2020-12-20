import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-right-click-menu',
  templateUrl: './right-click-menu.component.html',
  styleUrls: ['./right-click-menu.component.scss']
})
export class RightClickMenuComponent implements OnInit {

  @Input() x = 0;
  @Input() y = 0;
  @Input() label: string;

  constructor() { }

  ngOnInit(): void {
  }

}
