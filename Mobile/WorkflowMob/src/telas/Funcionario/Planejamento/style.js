import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.background,
  },
  scrollContent: {
    padding: 16,
  },
  titulo: {
    fontSize: 30,
    fontFamily: fonts.text,
    color: theme.text,
    fontWeight: 'bold',
    padding: 10,
  },
  areabotao: {
    padding: 10,
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    alignContent:"space-around"
    
  },
  botao: {
    paddingVertical: 10,
    paddingHorizontal: 15,
    marginHorizontal: 8,
    borderRadius: 15,
    alignItems: 'center',
    justifyContent: 'center'
  },
  textobotao: {
    fontSize: 13,
    fontFamily: fonts.text,
  },
  navinput: {
    width: '100%',
    padding: 10,
    fontSize: 17,
    fontFamily: fonts.text,
    backgroundColor: '#1C58F2',
    borderRadius: 10,
    borderBottomWidth: 0.1,
    borderBottomColor: '#000',
    marginBottom: 15,
    color: '#fff',
  },
  flat: {
    flex: 1,
  },
  containertarefas: {
    backgroundColor: theme.inputBackground,
    borderRadius: 10,
    padding: 15,
    marginBottom: 20,
  },
  linhaTarefa: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 10,
    position: 'relative'
  },
  textosTarefa: {
    marginLeft: 10,
    flexShrink: 1,
    flex: 1,
  },
  imag: {
    width: 60,
    height: 60,
  },
  textolistatitulo: {
    color: theme.text,
    fontSize: 15,
    fontFamily: fonts.text,
    fontWeight: "500",
  },
  textolista: {
    color: theme.text,
    fontSize: 15,
    fontFamily: fonts.text,
  },
  linhaInfo: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 5,
  },
  textolistacargo: {
    color: theme.text,
    fontSize: 13,
    fontFamily: fonts.text,
    backgroundColor: theme.inputBackground2,
    borderRadius: 15,
    paddingHorizontal: 10,
    paddingVertical: 5,
  },
  textolistadata: {
    color: theme.text,
    fontSize: 14,
    fontFamily: fonts.text,
    marginTop:4,
  },
  containerfiltro:{
    paddingHorizontal: 8,
    borderRadius: 8,
  },
  textofiltro:{
    color: "#fff",
    fontSize: 13,
    fontFamily: fonts.text,
    padding:5,
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    padding: 20,
  },
  modalContent: {
    backgroundColor: theme.background,
    borderRadius: 15,
    padding: 20,
    width: '90%',
    maxHeight: '80%',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
  modalTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: theme.text,
    marginBottom: 15,
    textAlign: 'center',
  },
  
  // Progress Bar Styles
  progressContainer: {
    marginBottom: 20,
    padding: 10,
    backgroundColor: theme.inputBackground,
    borderRadius: 10,
  },
  progressBar: {
    height: 12,
    backgroundColor: '#e0e0e0',
    borderRadius: 6,
    overflow: 'hidden',
    marginBottom: 5,
  },
  progressFill: {
    height: '100%',
    borderRadius: 6,
    transition: 'width 0.3s ease',
  },
  progressText: {
    fontSize: 12,
    color: theme.text,
    textAlign: 'center',
  },
  
  // Pages List Styles
  pagesList: {
    maxHeight: 300,
    marginBottom: 15,
  },
  pageItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: 15,
    borderBottomWidth: 1,
    borderBottomColor: theme.inputBackground,
    borderRadius: 8,
    marginBottom: 5,
    backgroundColor: theme.cardBackground,
  },
  pageItemRead: {
    backgroundColor: 'rgba(76, 175, 80, 0.1)',
    borderLeftWidth: 4,
    borderLeftColor: '#4CAF50',
  },
  pageText: {
    color: theme.text,
    fontSize: 14,
    fontWeight: '500',
  },
  pageStatus: {
    color: '#4CAF50',
    fontWeight: 'bold',
    fontSize: 16,
  },
  
  // Close Button
  closeButton: {
    backgroundColor: theme.primary,
    padding: 15,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 10,
  },
  closeButtonText: {
    color: '#fff',
    fontWeight: 'bold',
    fontSize: 16,
  },
  
  // PDF Modal Styles
  pdfModalContent: {
    backgroundColor: theme.background,
    borderRadius: 15,
    width: '95%',
    height: '90%',
    overflow: 'hidden',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
  pdfHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: 15,
    borderBottomWidth: 1,
    borderBottomColor: theme.inputBackground,
    backgroundColor: theme.cardBackground,
  },
  pdfTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: theme.text,
    flex: 1,
  },
  closePdfButton: {
    padding: 8,
    borderRadius: 20,
    backgroundColor: '#ff4444',
    width: 30,
    height: 30,
    alignItems: 'center',
    justifyContent: 'center',
  },
  closePdfButtonText: {
    color: '#fff',
    fontWeight: 'bold',
    fontSize: 14,
  },
  pdfContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 10,
  },
  pdfWrapper: {
    width: '100%',
    height: '100%',
    alignItems: 'center',
    justifyContent: 'center',
  },
  pdfPlaceholder: {
    fontSize: 18,
    color: theme.text,
    textAlign: 'center',
    marginBottom: 20,
  },
  pdfUrl: {
    fontSize: 12,
    color: theme.text3,
    textAlign: 'center',
    marginBottom: 30,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  loadingText: {
    fontSize: 16,
    color: theme.text,
  },
  
  // Navigation Controls
  navigationControls: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: '100%',
    paddingHorizontal: 20,
    position: 'absolute',
    bottom: 20,
  },
  navButton: {
    backgroundColor: theme.primary,
    paddingHorizontal: 20,
    paddingVertical: 12,
    borderRadius: 8,
    minWidth: 120,
    alignItems: 'center',
  },
  navButtonDisabled: {
    backgroundColor: '#cccccc',
  },
  navButtonText: {
    color: '#fff',
    fontWeight: 'bold',
    fontSize: 14,
  },
  
  // PDF Footer
  pdfFooter: {
    padding: 15,
    borderTopWidth: 1,
    borderTopColor: theme.inputBackground,
    backgroundColor: theme.cardBackground,
  },
  footerProgress: {
    alignItems: 'center',
  },
  footerProgressText: {
    fontSize: 14,
    color: theme.text,
    marginBottom: 5,
  },
  footerProgressBar: {
    height: 6,
    backgroundColor: '#e0e0e0',
    borderRadius: 3,
    width: '100%',
    overflow: 'hidden',
  },
  footerProgressFill: {
    height: '100%',
    backgroundColor: '#4CAF50',
    borderRadius: 3,
    transition: 'width 0.3s ease',
  },
  pageItemContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 5,
  },
  downloadButton: {
    backgroundColor: '#4CAF50',
    padding: 10,
    borderRadius: 5,
    marginLeft: 10,
  },
  downloadButtonText: {
    color: '#fff',
    fontWeight: 'bold',
    fontSize: 14,
  },
  pageItemWithActions: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: 15,
    borderBottomWidth: 1,
    borderBottomColor: theme.inputBackground,
    backgroundColor: theme.cardBackground,
    borderRadius: 8,
    marginBottom: 8,
  },
  pageInfo: {
    flex: 1,
  },
  pageNumber: {
    color: theme.text,
    fontSize: 16,
    fontWeight: '500',
  },
  pageStatus: {
    color: '#4CAF50',
    fontSize: 12,
    marginTop: 4,
  },
  pageActions: {
    flexDirection: 'row',
    gap: 8,
  },
  actionButton: {
    backgroundColor: '#1A5CFF',
    paddingHorizontal: 12,
    paddingVertical: 8,
    borderRadius: 6,
  },
  downloadAction: {
    backgroundColor: '#4CAF50',
  },
  actionButtonText: {
    color: '#fff',
    fontSize: 12,
    fontWeight: '500',
  },
  nomeTarefa: {
    color: theme.text,
    fontSize: 16,
    fontFamily: fonts.text,
    fontWeight: "bold",
    marginBottom: 3,
  },
  linhaTituloStatus: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    width: "100%",
  },
  dificuldadeContainer: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 4,
    marginRight: 8,
  },

});