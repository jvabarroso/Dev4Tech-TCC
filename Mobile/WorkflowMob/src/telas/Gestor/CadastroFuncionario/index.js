import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput, Alert,} from 'react-native';
import { showMessage } from "react-native-flash-message";
import DateTimePickerModal from "react-native-modal-datetime-picker";
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';

import api from '../../../../services/api';

export default function CadastroFuncionario({navigation, route}){
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const usuario = route.params?.usuario;
    const [sucess, setSucess] = useState(false); 

    const [nome, setNome] = useState('');
    const [senha, setSenha] = useState('');
    const [email, setEmail] = useState('');
    const [dataNascimento, setDataNascimento] = useState('');
    const [isDatePickerVisible, setDatePickerVisibility] = useState(false);
    const [cargo, setCargo] = useState('');
    const [cpf, setCpf] = useState('');
    const [telefone, setTelefone] = useState('');
    const [endereco, setEndereco] = useState('');
    const [numero, setNumero] = useState('');

    const campos = {
        nome,
        senha,
        email,
        dataNascimento,
        cargo,
        cpf,
        telefone,
        endereco,
        numero
    };

    const showDatePicker = () => setDatePickerVisibility(true);
    const hideDatePicker = () => setDatePickerVisibility(false);

    // Função chamada quando o usuário escolhe a data
    const handleConfirm = (date) => {
        const formatada = date.toLocaleDateString("pt-BR");
        setDataNascimento(formatada);
        hideDatePicker();
    };

    function limparCampos(){
        setNome('');
        setSenha('');
        setEmail('');
        setDataNascimento('');
        setCargo('');
        setCpf('');
        setTelefone('');
        setEndereco('');
        setNumero('');
    }


    async function cadastrar() {      
        const camposVazios = Object.entries(campos).filter(([_, valor]) => !valor.trim());      
        if (camposVazios.length > 0) {
            showMessage({
                message: "Erro Preencha todos os campos obrigatórios!",
                description: "Preencha todas as informações",
                floating: true,
                statusBarHeight: 70,
                type: "danger",
                duration: 2000,             
            });    
            return;
        }
        const cpfSemMascara = formatarCpfParaBanco(cpf);
        
        try {
            const res = await api.post('dev4tech/cadastrofunc.php', {
                Nome : nome, 
                Cargo :cargo, 
                DataNascimento : formatarDataParaBanco(dataNascimento), 
                Telefone : telefone, 
                Email : email, 
                CPF : cpfSemMascara, 
                Senha : senha, 
                endereco : endereco, 
                numero : numero,
                id_empresa: usuario.id_empresa,
                id_administradores: usuario.AdminId // Use o ID do usuário logado
            });

            if (res.data.sucesso === false) {

            showMessage({
                message: "Erro ao Cadastrar",
                description: res.data.mensagem,
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 3000,                    
            });  
            limparCampos();            
            return;
            }

            setSucess(true);
                showMessage({
                message: "Cadastrado com Sucesso",
                description: "Registro Cadastrado",
                floating: true,
                statusBarHeight: 70,
                type: "success",
                duration: 2000,             
            });         
            limparCampos();  
            } 
        catch (error) {
            console.log("ERRO NO CADASTRO:", error.message);
            if (error.response) {
                console.log("RESPOSTA DO SERVIDOR:", error.response.data);
            }
            if (error.request) {
                console.log("SEM RESPOSTA, REQUEST:", error.request);
            }
            setSucess(false);
            showMessage({
                message: "Ops Alguma coisa deu errado, tente novamente.",
                description: res.data.mensagem,
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 3000,                    
            });  
        }
        
    }   

    //Máscara input
    // Adicione estas funções utilitárias no topo do arquivo
    function formatarDataParaBanco(data) {
      if (!data) return '';
      
      // Se já está no formato do banco, retorna direto
      if (/^\d{4}-\d{2}-\d{2}$/.test(data)) return data;
      
      // Converte de DD/MM/AAAA para AAAA-MM-DD
      const partes = data.split('/');
      if (partes.length === 3) {
        return `${partes[2]}-${partes[1]}-${partes[0]}`;
      }
      return data;
    }

    function formatarTelefone(telefone) {
      if (!telefone) return '';
      // Remove todos os caracteres não numéricos
      return telefone.replace(/\D/g, '');
    }

    function formatarDataInput(text) {
      let data = text.replace(/\D/g, '');
      
      if (data.length > 2) data = `${data.slice(0,2)}/${data.slice(2)}`;
      if (data.length > 5) data = `${data.slice(0,5)}/${data.slice(5,9)}`;
      
      return data.slice(0,10);
    }

    function formatarTelefoneInput(text) {
      let tel = text.replace(/\D/g, '');
      
      if (tel.length > 0) tel = `(${tel}`;
      if (tel.length > 3) tel = `${tel.slice(0,3)}) ${tel.slice(3)}`;
      if (tel.length > 10) tel = `${tel.slice(0,10)}-${tel.slice(10,15)}`;
      
      return tel.slice(0,15);
    }

    function formatarCpfInput(text) {
        const digits = text.replace(/\D/g, '').slice(0, 11);
        if (digits.length <= 3) return digits;
        if (digits.length <= 6) return `${digits.slice(0,3)}.${digits.slice(3)}`;
        if (digits.length <= 9) return `${digits.slice(0,3)}.${digits.slice(3,6)}.${digits.slice(6)}`;
        return `${digits.slice(0,3)}.${digits.slice(3,6)}.${digits.slice(6,9)}-${digits.slice(9,11)}`;
    }

    function formatarCpfParaBanco(maskedCpf) {
        if (!maskedCpf) return '';
        return maskedCpf.replace(/\D/g, '');
    }


    return(
   <View style={styles.container}>
        <ScrollView contentContainerStyle={styles.scrollContent}>
            <Text style={styles.titulo}>Cadastrar Funcionário</Text>
                    <View style={styles.areaInput}>
                        <Text style={styles.texto}>Nome do funcionário</Text>
                        <TextInput
                            style={styles.input}
                            value={nome}
                            placeholder="Gabriel Kenzo" //depois mudar, mensagem para mim mesmo dnv :D
                            placeholderTextColor={theme.text3}
                            onChangeText={(text) => setNome(text)}
                        />
                        <Text style={styles.texto}>Senha</Text>
                        <TextInput
                            style={styles.input}
                            value={senha}
                            placeholder="1234"
                            placeholderTextColor={theme.text3}
                            onChangeText={(text) => setSenha(text)}
                        />
                        <Text style={styles.texto}>Data de Nascimento</Text>
                        <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                            <TextInput
                                style={[styles.input, { marginRight: 5, width: '77%' }]}
                                placeholder="00/00/0000"
                                placeholderTextColor={theme.text3}
                                keyboardType="numeric"
                                value={dataNascimento}
                                onChangeText={(text) => setDataNascimento(formatarDataInput(text))}
                                maxLength={10}
                            />
                            <TouchableOpacity
                                onPress={showDatePicker}
                                style={ styles.databotao}
                            >
                                <Ionicons name="calendar-outline" size={22} color={theme.text} />
                            </TouchableOpacity>
                        </View>

                        <DateTimePickerModal
                            isVisible={isDatePickerVisible}
                            mode="date"
                            display="default"
                            themeVariant={theme.mode === 'dark' ? 'dark' : 'light'}
                            onConfirm={handleConfirm}
                            onCancel={hideDatePicker}
                            locale="pt-BR"
                            confirmTextIOS="Confirmar"
                            cancelTextIOS="Cancelar"
                        />
                        <Text style={styles.texto}>Email</Text>
                        <TextInput
                            style={styles.input}
                            value={email}
                            placeholder="joaovitinhocraft@gmail.com"
                            placeholderTextColor={theme.text3}
                            onChangeText={(text) => setEmail(text)}
                        />
                        <Text style={styles.texto}>Telefone</Text>
                        <TextInput
                            style={styles.input}
                            value={telefone}
                            placeholder="(13) 99899989"
                            placeholderTextColor={theme.text3}
                            onChangeText={(text) => setTelefone(formatarTelefoneInput(text))}
                        />
                        <Text style={styles.texto}>CPF</Text>
                        <TextInput
                            style={styles.input}
                            value={cpf}
                            placeholder="123.456.789-09"
                            placeholderTextColor={theme.text3}
                            onChangeText={(text) => setCpf(formatarCpfInput(text))}
                        />
                        <View style={styles.linha}>
                            <Text style={styles.textoe}>Endereço</Text> 
                            <Text style={styles.texton}>Numero</Text>
                        </View>  
                        <View style={styles.linha}>
                            <TextInput
                                style={styles.inputendereco}
                                value={endereco}
                                placeholder="Rua João da Fonseca, Jardim Mato Grosso, Cananeia senha"
                                placeholderTextColor={theme.text3}
                                onChangeText={(text) => setEndereco(text)}
                            />   
                            <TextInput
                                style={styles.inputnum}
                                value={numero}
                                placeholder="123"
                                placeholderTextColor={theme.text3}
                                onChangeText={(text) => setNumero(text)}
                            />   
                        </View>

                        <Text style={styles.texto}>Categoria do funcionário</Text>
                        <TextInput
                            style={styles.input}
                            value={cargo}
                            placeholder="Analista"
                            placeholderTextColor={theme.text3}
                            onChangeText={(text) => setCargo(text)}
                        />
                    </View>
                <TouchableOpacity 
                    style={styles.botaocriar}
                    onPress={cadastrar} 
                >
                    <Text style={styles.textocriar}>Criar</Text>
                </TouchableOpacity>
            </ScrollView>
        </View>
    );
}