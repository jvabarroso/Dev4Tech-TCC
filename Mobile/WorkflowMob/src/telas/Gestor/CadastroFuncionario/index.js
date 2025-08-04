import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'


import { Ionicons } from '@expo/vector-icons';

export default function CadastroFuncionario({navigation}){
    const { theme } = useTheme();
    const styles = getStyles(theme);
    
    const [nome, setNome] = useState('');
    const [senha, setSenha] = useState('');
    const [email, setEmail] = useState('');
    const [dataNascimento, setDataNascimento] = useState('');
    const [cargo, setCargo] = useState('');
    const [telefone, setTelefone] = useState('');
    const [endereco, setEndereco] = useState('');
    const [numero, setNumero] = useState('');

    const campos = {
        nome,
        senha,
        email,
        dataNascimento,
        cargo,
        telefone,
        endereco,
        numero
    };

    const [equipes, setequipes] = useState(true);
    const [equipeselecionada, setEquipeselecionada] = useState(null);

    const cliqueinformacao = () => 
        {
            setequipes(valorAtual => !valorAtual); 
        };
    
    const confirmodeequipe = (equipeSelecionada) => 
        {   
            setEquipeselecionada(equipeSelecionada.titulo);
            setequipes(valorAtual => !valorAtual); 
        };

    async function cadastrar() {      
        const camposVazios = Object.entries(campos).filter(([_, valor]) => !valor.trim());      
        if (camposVazios.length > 0) {
            Alert.alert("Erro", "Preencha todos os campos obrigatórios!");
            return;
        }
        try {
            const obj = {
                
                Nome : nome, 
                Cargo :cargo, 
                DataNascimento : dataNascimento, 
                Telefone : telefone, 
                Email : email, 
                Senha : senha, 
                endereco : endereco, 
                numero : numero    
            }

            const res = await api.post('dev4tec/cadastrofunc.php', obj);

            if (res.data.sucesso === false) {
            showMessage({
                message: "Erro ao Salvar",
                description: res.data.mensagem,
                type: "warning",
                duration: 3000,                    
            });  
            limparCampos();            
            return;
            }

            setSucess(true);
                showMessage({
                message: "Salvo com Sucesso",
                description: "Registro Salvo",
                type: "success",
                duration: 800,             
            });          
                
            } 
        catch (error) {
            Alert.alert("Ops", "Alguma coisa deu errado, tente novamente.");
            setSucess(false);
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
                            placeholderTextColor={theme.text}
                            onChangeText={(text) => setNome(text)}
                        />
                        <Text style={styles.texto}>Senha</Text>
                        <TextInput
                            style={styles.input}
                            value={senha}
                            placeholder="1234"
                            placeholderTextColor={theme.text}
                            onChangeText={(text) => setSenha(text)}
                        />
                        <Text style={styles.texto}>Data de Nascimento</Text>
                        <TextInput
                            style={styles.input}
                            value={dataNascimento}
                            placeholder="25/25/2525"
                            placeholderTextColor={theme.text}
                            onChangeText={(text) => setDataNascimento(formatarDataInput(text))}
                        />
                        <Text style={styles.texto}>Email</Text>
                        <TextInput
                            style={styles.input}
                            value={email}
                            placeholder="joaovitinhocraft@gmail.com"
                            placeholderTextColor={theme.text}
                            onChangeText={(text) => setEmail(text)}
                        />
                        <Text style={styles.texto}>Telefone</Text>
                        <TextInput
                            style={styles.input}
                            value={telefone}
                            placeholder="(13) 99899989"
                            placeholderTextColor={theme.text}
                            onChangeText={(text) => setTelefone(formatarTelefoneInput(text))}
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
                                placeholderTextColor={theme.text}
                                onChangeText={(text) => setEndereco(text)}
                            />   
                            <TextInput
                                style={styles.inputnum}
                                value={numero}
                                placeholder="123"
                                placeholderTextColor={theme.text}
                                onChangeText={(text) => setNumero(text)}
                            />   
                        </View>

                        <Text style={styles.texto}>Categoria do funcionário</Text>
                        <TextInput
                            style={styles.input}
                            value={cargo}
                            placeholder="Analista"
                            placeholderTextColor={theme.text}
                            onChangeText={(text) => setCargo(text)}
                        />
                        <Text style={styles.texto}>Adicionar a uma equipe</Text>
                        <TouchableOpacity
                            style={[styles.input, styles.linha]}
                            onPress={cliqueinformacao}
                        >
                            <Text style={styles.textobotao}>{equipeselecionada || "Selecione uma equipe"}</Text> 
                            <Ionicons name="caret-down-outline" size={18} color="black" />
                        </TouchableOpacity>
                    </View>
                {!equipes && equipe.map(item => (
                    <TouchableOpacity
                        key={item.id}
                        style={styles.containerequipes}
                        onPress={() => confirmodeequipe(item)}
                    >
                    <Image source={item.imagem} style={styles.imag} />
                    <View style={styles.textos}>
                        <Text style={styles.textolistatitulo}>{item.titulo}</Text>
                        <Text style={styles.textolistacargo}>{item.cargo}</Text>
                    </View>
                    </TouchableOpacity>
                ))}
                <TouchableOpacity style={styles.botaocriar}>
                    <Text style={styles.textocriar}>Criar</Text>
                </TouchableOpacity>
            </ScrollView>
        </View>
    );
}