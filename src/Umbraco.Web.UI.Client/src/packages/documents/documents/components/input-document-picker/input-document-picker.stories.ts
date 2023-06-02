import { Meta, StoryObj } from '@storybook/web-components';
import './input-document-picker.element.js';
import type { UmbInputDocumentPickerElement } from './input-document-picker.element.js';

const meta: Meta<UmbInputDocumentPickerElement> = {
	title: 'Components/Inputs/Document Picker',
	component: 'umb-input-document-picker',
};

export default meta;
type Story = StoryObj<UmbInputDocumentPickerElement>;

export const Overview: Story = {
	args: {},
};
