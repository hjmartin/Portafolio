import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import { InfoCardComponent } from '../../../../shared/components/info-card/info-card.component';
import { SectionShellComponent } from '../../../../shared/components/section-shell/section-shell.component';

@Component({
  selector: 'app-projects-page',
  standalone: true,
  imports: [CommonModule, SectionShellComponent, InfoCardComponent],
  templateUrl: './projects-page.component.html',
  styleUrl: './projects-page.component.css'
})
export class ProjectsPageComponent {}
