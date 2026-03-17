import { Component, input } from '@angular/core';

@Component({
  selector: 'app-pill',
  standalone: true,
  template: '<span class="pill">{{ label() }}</span>',
  styleUrl: './pill.component.css'
})
export class PillComponent {
  readonly label = input.required<string>();
}
