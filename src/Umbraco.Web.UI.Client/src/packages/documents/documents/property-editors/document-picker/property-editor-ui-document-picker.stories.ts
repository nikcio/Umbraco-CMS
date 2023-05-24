import { Meta, Story } from '@storybook/web-components';
import { html } from '@umbraco-cms/backoffice/external/lit';

import type { UmbPropertyEditorUIContentPickerElement } from './property-editor-ui-document-picker.element.js';
import './property-editor-ui-document-picker.element';

export default {
	title: 'Property Editor UIs/Content Picker',
	component: 'umb-property-editor-ui-document-picker',
	id: 'umb-property-editor-ui-document-picker',
} as Meta;

export const AAAOverview: Story<UmbPropertyEditorUIContentPickerElement> = () =>
	html` <umb-property-editor-ui-document-picker></umb-property-editor-ui-document-picker>`;
AAAOverview.storyName = 'Overview';
